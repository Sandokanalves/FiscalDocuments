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
            IssueDate = fiscalDocument.IssueDate,
            IssuerName = fiscalDocument.IssuerName,
            IssuerCnpj = fiscalDocument.IssuerCnpj,
            IssuerAddress = new AddressDto
            {
                Street = fiscalDocument.IssuerAddress.Street,
                Number = fiscalDocument.IssuerAddress.Number,
                District = fiscalDocument.IssuerAddress.District,
                CityName = fiscalDocument.IssuerAddress.CityName,
                State = fiscalDocument.IssuerAddress.State,
                ZipCode = fiscalDocument.IssuerAddress.ZipCode
            },
            RecipientName = fiscalDocument.RecipientName,
            RecipientDocument = fiscalDocument.RecipientDocument,
            RecipientAddress = new AddressDto
            {
                Street = fiscalDocument.RecipientAddress.Street,
                Number = fiscalDocument.RecipientAddress.Number,
                District = fiscalDocument.RecipientAddress.District,
                CityName = fiscalDocument.RecipientAddress.CityName,
                State = fiscalDocument.RecipientAddress.State,
                ZipCode = fiscalDocument.RecipientAddress.ZipCode
            },
            TotalAmount = fiscalDocument.TotalAmount,
            Items = fiscalDocument.Items.Select(i => new ProductItemDto
            {
                ProductCode = i.ProductCode,
                Description = i.Description,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice
            }).ToList()
        };
    }
}

