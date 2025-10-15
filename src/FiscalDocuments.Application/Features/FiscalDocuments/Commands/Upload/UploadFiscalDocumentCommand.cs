using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Commands.Upload;

public class UploadFiscalDocumentCommand : IRequest<Guid>
{
    public string XmlContent { get; set; } = string.Empty;
}

