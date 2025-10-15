namespace FiscalDocuments.Domain.Entities;

public class FiscalDocument
{
    public Guid Id { get; private set; }
    public string AccessKey { get; private set; } = string.Empty;
    public string IssuerCnpj { get; private set; } = string.Empty;
    public string RecipientCnpj { get; private set; } = string.Empty;
    public DateTime IssueDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private FiscalDocument() { }

    public FiscalDocument(string accessKey, string issuerCnpj, string recipientCnpj, DateTime issueDate, decimal totalAmount)
    {
        Id = Guid.NewGuid();
        AccessKey = accessKey;
        IssuerCnpj = issuerCnpj;
        RecipientCnpj = recipientCnpj;
        IssueDate = issueDate;
        TotalAmount = totalAmount;
        CreatedAt = DateTime.UtcNow;
    }
}


