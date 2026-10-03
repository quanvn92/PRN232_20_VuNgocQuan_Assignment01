using _20_VuNgocQuan_Assignment01.Models;

namespace _20_VuNgocQuan_Assignment01.Repositories;

public interface INewsArticleRepository : IGenericRepository<NewsArticle>
{
    Task<NewsArticle?> GetByIdWithTagsAsync(string id);
    Task<IEnumerable<NewsArticle>> GetAllWithDetailsAsync();
    Task<IEnumerable<NewsArticle>> GetActiveNewsAsync();
    Task<IEnumerable<NewsArticle>> GetByCreatedByAsync(short accountId);
    Task<IEnumerable<NewsArticle>> SearchAsync(string? keyword);
    Task<IEnumerable<NewsArticle>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task UpdateTagsAsync(string newsArticleId, IEnumerable<int> tagIds);
}
