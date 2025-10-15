using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Commands.Delete;

public class DeleteFiscalDocumentCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

