using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Queries.GetById;

public class GetFiscalDocumentByIdQuery : IRequest<FiscalDocumentDto?>
{
    public Guid Id { get; set; }
}

