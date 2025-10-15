using FiscalDocuments.Domain.Entities;
using FiscalDocuments.Domain.Interfaces;
using FiscalDocuments.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FiscalDocuments.Infrastructure.Persistence.Repositories;

public class FiscalDocumentRepository : IFiscalDocumentRepository
{
    private readonly AppDbContext _context;

    public FiscalDocumentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FiscalDocument fiscalDocument, CancellationToken cancellationToken)
    {
        await _context.FiscalDocuments.AddAsync(fiscalDocument, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<FiscalDocument?> GetByAccessKeyAsync(string accessKey, CancellationToken cancellationToken)
    {
        return await _context.FiscalDocuments
            .FirstOrDefaultAsync(f => f.AccessKey == accessKey, cancellationToken);
    }

    public async Task<FiscalDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.FiscalDocuments.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<FiscalDocument>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.FiscalDocuments
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

