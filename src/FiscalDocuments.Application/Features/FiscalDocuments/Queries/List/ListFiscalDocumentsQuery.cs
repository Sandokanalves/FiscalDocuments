using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Queries.List;

public class ListFiscalDocumentsQuery : IRequest<IEnumerable<FiscalDocumentDto>>
{
}


