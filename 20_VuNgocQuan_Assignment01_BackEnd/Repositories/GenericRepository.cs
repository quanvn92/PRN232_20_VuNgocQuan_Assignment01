using _20_VuNgocQuan_Assignment01.DAOs;
using Microsoft.EntityFrameworkCore;

namespace _20_VuNgocQuan_Assignment01.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly FUNewsManagementDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(FUNewsManagementDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public virtual async Task<T?> GetByIdAsync(object id)
        => await _dbSet.FindAsync(id);

    public virtual async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public virtual async Task UpdateAsync(T entity)
        => _dbSet.Update(entity);

    public virtual async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task SaveAsync()
        => await _context.SaveChangesAsync();
}
