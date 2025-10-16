using System.Xml.Linq;
using FiscalDocuments.Application.Common.Interfaces;
using FiscalDocuments.Domain.Interfaces;
using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Commands.Upload;

public class UploadFiscalDocumentCommandHandler : IRequestHandler<UploadFiscalDocumentCommand, Guid>
{
    private readonly IFiscalDocumentRepository _repository;
    private readonly IXmlParserFactory _xmlParserFactory;

    public UploadFiscalDocumentCommandHandler(IFiscalDocumentRepository repository, IXmlParserFactory xmlParserFactory)
    {
        _repository = repository;
        _xmlParserFactory = xmlParserFactory;
    }

    public async Task<Guid> Handle(UploadFiscalDocumentCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
        {
            throw new ArgumentException("Arquivo inválido.", nameof(request.File));
        }

        using var reader = new StreamReader(request.File.OpenReadStream());
        var xmlContent = await reader.ReadToEndAsync(cancellationToken);
        var xmlDoc = XDocument.Parse(xmlContent);

        var parser = _xmlParserFactory.GetParser(xmlDoc);
        var fiscalDocument = parser.Parse(xmlDoc);

        var existingDocument = await _repository.GetByAccessKeyAsync(fiscalDocument.AccessKey, cancellationToken);
        if (existingDocument != null)
        {
            return existingDocument.Id;
        }

        await _repository.AddAsync(fiscalDocument, cancellationToken);

        return fiscalDocument.Id;
    }
}


