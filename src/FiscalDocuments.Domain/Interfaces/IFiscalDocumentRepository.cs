using FiscalDocuments.Domain.Entities;

namespace FiscalDocuments.Domain.Interfaces;

public interface IFiscalDocumentRepository
{
    Task AddAsync(FiscalDocument fiscalDocument, CancellationToken cancellationToken);
    Task<FiscalDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<FiscalDocument?> GetByAccessKeyAsync(string accessKey, CancellationToken cancellationToken);
    Task<IEnumerable<FiscalDocument>> ListAsync(CancellationToken cancellationToken);
}


