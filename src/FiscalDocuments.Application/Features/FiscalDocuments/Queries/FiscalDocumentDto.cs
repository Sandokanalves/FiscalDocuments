namespace FiscalDocuments.Application.Features.FiscalDocuments.Queries;

public class FiscalDocumentDto
{
    public Guid Id { get; set; }
    public string AccessKey { get; set; } = string.Empty;
    public string IssuerCnpj { get; set; } = string.Empty;
    public string RecipientCnpj { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public decimal TotalAmount { get; set; }
}

