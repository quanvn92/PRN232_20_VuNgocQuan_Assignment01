using _20_VuNgocQuan_Assignment01.DAOs;
using _20_VuNgocQuan_Assignment01.Models;
using Microsoft.EntityFrameworkCore;

namespace _20_VuNgocQuan_Assignment01.Repositories;

public class SystemAccountRepository : GenericRepository<SystemAccount>, ISystemAccountRepository
{
    public SystemAccountRepository(FUNewsManagementDbContext context) : base(context) { }

    public async Task<SystemAccount?> GetByEmailAsync(string email)
        => await _dbSet.FirstOrDefaultAsync(a => a.AccountEmail == email);

    public async Task<bool> HasCreatedArticlesAsync(short accountId)
        => await _context.NewsArticles.AnyAsync(n => n.CreatedByID == accountId);

    public async Task<IEnumerable<SystemAccount>> SearchAsync(string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await _dbSet.ToListAsync();

        return await _dbSet
            .Where(a => (a.AccountName != null && a.AccountName.Contains(keyword))
                     || (a.AccountEmail != null && a.AccountEmail.Contains(keyword)))
            .ToListAsync();
    }
}
