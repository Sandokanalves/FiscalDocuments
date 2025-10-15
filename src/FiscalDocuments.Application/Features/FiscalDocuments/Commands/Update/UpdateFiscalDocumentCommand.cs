using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Commands.Update;

public class UpdateFiscalDocumentCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
}

