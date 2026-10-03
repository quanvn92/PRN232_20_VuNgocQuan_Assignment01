using _20_VuNgocQuan_Assignment01.DAOs;
using _20_VuNgocQuan_Assignment01.Models;
using Microsoft.EntityFrameworkCore;

namespace _20_VuNgocQuan_Assignment01.Repositories;

public class TagRepository : GenericRepository<Tag>, ITagRepository
{
    public TagRepository(FUNewsManagementDbContext context) : base(context) { }

    public async Task<IEnumerable<Tag>> GetByIdsAsync(IEnumerable<int> ids)
        => await _dbSet.Where(t => ids.Contains(t.TagID)).ToListAsync();
}
