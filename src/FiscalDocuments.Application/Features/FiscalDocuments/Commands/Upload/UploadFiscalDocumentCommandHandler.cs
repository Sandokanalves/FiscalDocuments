using System.Xml.Linq;
using FiscalDocuments.Domain.Entities;
using FiscalDocuments.Domain.Interfaces;
using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Commands.Upload;

public class UploadFiscalDocumentCommandHandler : IRequestHandler<UploadFiscalDocumentCommand, Guid>
{
    private readonly IFiscalDocumentRepository _repository;

    public UploadFiscalDocumentCommandHandler(IFiscalDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(UploadFiscalDocumentCommand request, CancellationToken cancellationToken)
    {
        var xmlDoc = XDocument.Parse(request.XmlContent);
        var ns = xmlDoc.Root?.GetDefaultNamespace();

        var ide = xmlDoc.Descendants(ns + "ide").FirstOrDefault();
        var emit = xmlDoc.Descendants(ns + "emit").FirstOrDefault();
        var dest = xmlDoc.Descendants(ns + "dest").FirstOrDefault();
        var total = xmlDoc.Descendants(ns + "ICMSTot").FirstOrDefault();

        var accessKey = xmlDoc.Descendants(ns + "infNFe").FirstOrDefault()?.Attribute("Id")?.Value.Replace("NFe", "") ?? Guid.NewGuid().ToString();
        var issuerCnpj = emit?.Element(ns + "CNPJ")?.Value ?? "N/A";
        var recipientCnpj = dest?.Element(ns + "CNPJ")?.Value ?? "N/A";
        var issueDate = DateTime.Parse(ide?.Element(ns + "dhEmi")?.Value ?? DateTime.UtcNow.ToString());
        var totalAmount = decimal.Parse(total?.Element(ns + "vNF")?.Value ?? "0");

        var existingDocument = await _repository.GetByAccessKeyAsync(accessKey, cancellationToken);
        if (existingDocument != null)
        {
            return existingDocument.Id;
        }

        var fiscalDocument = new FiscalDocument(
            accessKey,
            issuerCnpj,
            recipientCnpj,
            issueDate,
            totalAmount);

        await _repository.AddAsync(fiscalDocument, cancellationToken);

        return fiscalDocument.Id;
    }
}


