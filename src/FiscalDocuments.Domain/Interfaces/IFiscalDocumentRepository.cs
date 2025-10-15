using FiscalDocuments.Domain.Entities;

namespace FiscalDocuments.Domain.Interfaces;

public interface IFiscalDocumentRepository
{
    Task AddAsync(FiscalDocument fiscalDocument, CancellationToken cancellationToken);
    Task<FiscalDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<FiscalDocument?> GetByAccessKeyAsync(string accessKey, CancellationToken cancellationToken);
    Task<IEnumerable<FiscalDocument>> ListAsync(int pageNumber, int pageSize, string? issuerCnpj, CancellationToken cancellationToken);
    Task UpdateAsync(FiscalDocument fiscalDocument, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}




