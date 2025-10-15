using FiscalDocuments.Domain.ValueObjects;

namespace FiscalDocuments.Domain.Entities;

public class FiscalDocument
{
    public Guid Id { get; private set; }
    public string AccessKey { get; private set; } = string.Empty;
    public int Model { get; private set; }
    public int Series { get; private set; }
    public int DocumentNumber { get; private set; }
    public DateTime IssueDate { get; private set; }

    public string IssuerName { get; private set; } = string.Empty;
    public string IssuerCnpj { get; private set; } = string.Empty;
    public Address IssuerAddress { get; private set; } = null!;

    public string RecipientName { get; private set; } = string.Empty;
    public string RecipientDocument { get; private set; } = string.Empty;
    public Address RecipientAddress { get; private set; } = null!;

    public decimal TotalProducts { get; private set; }
    public decimal TotalAmount { get; private set; }

    private readonly List<ProductItem> _items = new();
    public IReadOnlyCollection<ProductItem> Items => _items.AsReadOnly();

    public DateTime CreatedAt { get; private set; }

    private FiscalDocument() { }

    public void AddItem(ProductItem item)
    {
        _items.Add(item);
    }

    public void UpdateTotalAmount(decimal newTotalAmount)
    {
        if (newTotalAmount < 0)
        {
            throw new ArgumentException("O valor total não pode ser negativo.", nameof(newTotalAmount));
        }
        TotalAmount = newTotalAmount;
    }

    public FiscalDocument(
        string accessKey, int model, int series, int documentNumber, DateTime issueDate,
        string issuerName, string issuerCnpj, Address issuerAddress,
        string recipientName, string recipientDocument, Address recipientAddress,
        decimal totalProducts, decimal totalAmount)
    {
        Id = Guid.NewGuid();
        AccessKey = accessKey;
        Model = model;
        Series = series;
        DocumentNumber = documentNumber;
        IssueDate = issueDate;
        IssuerName = issuerName;
        IssuerCnpj = issuerCnpj;
        IssuerAddress = issuerAddress;
        RecipientName = recipientName;
        RecipientDocument = recipientDocument;
        RecipientAddress = recipientAddress;
        TotalProducts = totalProducts;
        TotalAmount = totalAmount;
        CreatedAt = DateTime.UtcNow;
    }
}




