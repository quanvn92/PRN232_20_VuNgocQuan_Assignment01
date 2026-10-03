using _20_VuNgocQuan_Assignment01.DAOs;
using _20_VuNgocQuan_Assignment01.Models;
using Microsoft.EntityFrameworkCore;

namespace _20_VuNgocQuan_Assignment01.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(FUNewsManagementDbContext context) : base(context) { }

    public async Task<bool> HasNewsArticlesAsync(short categoryId)
        => await _context.NewsArticles.AnyAsync(n => n.CategoryID == categoryId);

    public async Task<IEnumerable<Category>> SearchAsync(string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await _dbSet.Include(c => c.ParentCategory).ToListAsync();

        return await _dbSet
            .Include(c => c.ParentCategory)
            .Where(c => c.CategoryName.Contains(keyword)
                     || c.CategoryDesciption.Contains(keyword))
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetActiveAsync()
        => await _dbSet
            .Include(c => c.ParentCategory)
            .Where(c => c.IsActive == true)
            .ToListAsync();

    public override async Task<IEnumerable<Category>> GetAllAsync()
        => await _dbSet.Include(c => c.ParentCategory).ToListAsync();
}
