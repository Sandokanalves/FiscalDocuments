using FiscalDocuments.Domain.ValueObjects;

namespace FiscalDocuments.Application.Features.FiscalDocuments.Queries;

public class AddressDto
{
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}

public class ProductItemDto
{
    public string ProductCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class FiscalDocumentDto
{
    public Guid Id { get; set; }
    public string AccessKey { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }

    public string IssuerName { get; set; } = string.Empty;
    public string IssuerCnpj { get; set; } = string.Empty;
    public AddressDto IssuerAddress { get; set; } = null!;

    public string RecipientName { get; set; } = string.Empty;
    public string RecipientDocument { get; set; } = string.Empty;
    public AddressDto RecipientAddress { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public List<ProductItemDto> Items { get; set; } = new();
}

