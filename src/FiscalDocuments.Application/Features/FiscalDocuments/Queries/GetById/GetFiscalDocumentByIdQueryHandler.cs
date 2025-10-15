using FiscalDocuments.Domain.Interfaces;
using MediatR;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Queries.GetById;

public class GetFiscalDocumentByIdQueryHandler : IRequestHandler<GetFiscalDocumentByIdQuery, FiscalDocumentDto?>
{
    private readonly IFiscalDocumentRepository _repository;

    public GetFiscalDocumentByIdQueryHandler(IFiscalDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<FiscalDocumentDto?> Handle(GetFiscalDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var fiscalDocument = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (fiscalDocument is null)
        {
            return null;
        }

        return new FiscalDocumentDto
        {
            Id = fiscalDocument.Id,
            AccessKey = fiscalDocument.AccessKey,
            IssuerCnpj = fiscalDocument.IssuerCnpj,
            RecipientCnpj = fiscalDocument.RecipientCnpj,
            IssueDate = fiscalDocument.IssueDate,
            TotalAmount = fiscalDocument.TotalAmount
        };
    }
}

