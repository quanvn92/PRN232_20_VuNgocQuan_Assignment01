using _20_VuNgocQuan_Assignment01.Models;

namespace _20_VuNgocQuan_Assignment01.Repositories;

public interface ITagRepository : IGenericRepository<Tag>
{
    Task<IEnumerable<Tag>> GetByIdsAsync(IEnumerable<int> ids);
}
