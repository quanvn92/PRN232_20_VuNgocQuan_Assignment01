using _20_VuNgocQuan_Assignment01.Models;

namespace _20_VuNgocQuan_Assignment01.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<bool> HasNewsArticlesAsync(short categoryId);
    Task<IEnumerable<Category>> SearchAsync(string? keyword);
    Task<IEnumerable<Category>> GetActiveAsync();
}
