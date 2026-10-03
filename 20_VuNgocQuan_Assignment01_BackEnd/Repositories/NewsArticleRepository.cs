using _20_VuNgocQuan_Assignment01.DAOs;
using _20_VuNgocQuan_Assignment01.Models;
using Microsoft.EntityFrameworkCore;

namespace _20_VuNgocQuan_Assignment01.Repositories;

public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
{
    public NewsArticleRepository(FUNewsManagementDbContext context) : base(context) { }

    public async Task<NewsArticle?> GetByIdWithTagsAsync(string id)
        => await _dbSet
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .FirstOrDefaultAsync(n => n.NewsArticleID == id);

    public async Task<IEnumerable<NewsArticle>> GetAllWithDetailsAsync()
        => await _dbSet
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .ToListAsync();

    public async Task<IEnumerable<NewsArticle>> GetActiveNewsAsync()
        => await _dbSet
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Where(n => n.NewsStatus == true)
            .ToListAsync();

    public async Task<IEnumerable<NewsArticle>> GetByCreatedByAsync(short accountId)
        => await _dbSet
            .Include(n => n.Category)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Where(n => n.CreatedByID == accountId)
            .ToListAsync();

    public async Task<IEnumerable<NewsArticle>> SearchAsync(string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await GetAllWithDetailsAsync();

        return await _dbSet
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Where(n => (n.NewsTitle != null && n.NewsTitle.Contains(keyword))
                     || n.Headline.Contains(keyword)
                     || (n.NewsContent != null && n.NewsContent.Contains(keyword)))
            .ToListAsync();
    }

    public async Task<IEnumerable<NewsArticle>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        => await _dbSet
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();

    public async Task UpdateTagsAsync(string newsArticleId, IEnumerable<int> tagIds)
    {
        // Remove existing tags
        var existing = _context.NewsTags.Where(nt => nt.NewsArticleID == newsArticleId);
        _context.NewsTags.RemoveRange(existing);

        // Add new tags
        foreach (var tagId in tagIds)
        {
            await _context.NewsTags.AddAsync(new NewsTag
            {
                NewsArticleID = newsArticleId,
                TagID = tagId
            });
        }
    }

    public override async Task<NewsArticle?> GetByIdAsync(object id)
        => await GetByIdWithTagsAsync(id.ToString()!);
}
