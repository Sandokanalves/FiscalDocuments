using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Queries.List;

public class ListFiscalDocumentsQuery : IRequest<IEnumerable<FiscalDocumentDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? IssuerCnpj { get; set; }
}



