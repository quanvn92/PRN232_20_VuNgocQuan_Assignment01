using _20_VuNgocQuan_Assignment01.Models;

namespace _20_VuNgocQuan_Assignment01.Repositories;

public interface ISystemAccountRepository : IGenericRepository<SystemAccount>
{
    Task<SystemAccount?> GetByEmailAsync(string email);
    Task<bool> HasCreatedArticlesAsync(short accountId);
    Task<IEnumerable<SystemAccount>> SearchAsync(string? keyword);
}
