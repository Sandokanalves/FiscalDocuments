namespace FiscalDocuments.Domain.Entities;

public class ProductItem
{
    public Guid Id { get; private set; }
    public string ProductCode { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Ncm { get; private set; } = string.Empty;
    public int Cfop { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice { get; private set; }

    public Guid FiscalDocumentId { get; private set; }
    public virtual FiscalDocument FiscalDocument { get; private set; } = null!;

    private ProductItem() { }

    public ProductItem(string productCode, string description, string ncm, int cfop, decimal quantity, decimal unitPrice, decimal totalPrice)
    {
        Id = Guid.NewGuid();
        ProductCode = productCode;
        Description = description;
        Ncm = ncm;
        Cfop = cfop;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = totalPrice;
    }
}

