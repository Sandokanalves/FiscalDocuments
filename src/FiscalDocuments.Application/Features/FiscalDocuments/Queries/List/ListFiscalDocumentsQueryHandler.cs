using FiscalDocuments.Domain.Interfaces;
using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Queries.List;

public class ListFiscalDocumentsQueryHandler : IRequestHandler<ListFiscalDocumentsQuery, IEnumerable<FiscalDocumentDto>>
{
    private readonly IFiscalDocumentRepository _repository;

    public ListFiscalDocumentsQueryHandler(IFiscalDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<FiscalDocumentDto>> Handle(ListFiscalDocumentsQuery request, CancellationToken cancellationToken)
    {
        var fiscalDocuments = await _repository.ListAsync(request.PageNumber, request.PageSize, request.IssuerCnpj, cancellationToken);

        return fiscalDocuments.Select(doc => new FiscalDocumentDto
        {
            Id = doc.Id,
            AccessKey = doc.AccessKey,
            IssueDate = doc.IssueDate,
            IssuerName = doc.IssuerName,
            IssuerCnpj = doc.IssuerCnpj,
            RecipientName = doc.RecipientName,
            RecipientDocument = doc.RecipientDocument,
            TotalAmount = doc.TotalAmount
        });
    }
}



