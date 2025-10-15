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
        return await _context.FiscalDocuments
            .Include(f => f.Items)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<FiscalDocument>> ListAsync(int pageNumber, int pageSize, string? issuerCnpj, CancellationToken cancellationToken)
    {
        var query = _context.FiscalDocuments.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(issuerCnpj))
        {
            query = query.Where(f => f.IssuerCnpj == issuerCnpj);
        }

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(FiscalDocument fiscalDocument, CancellationToken cancellationToken)
    {
        _context.FiscalDocuments.Update(fiscalDocument);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var document = await _context.FiscalDocuments.FindAsync(new object[] { id }, cancellationToken);
        if (document != null)
        {
            _context.FiscalDocuments.Remove(document);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}




