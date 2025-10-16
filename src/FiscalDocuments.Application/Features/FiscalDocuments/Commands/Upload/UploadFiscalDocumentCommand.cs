using MediatR;
using Microsoft.AspNetCore.Http;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Commands.Upload;

public class UploadFiscalDocumentCommand : IRequest<Guid>
{
    public IFormFile File { get; set; } = null!;
}

