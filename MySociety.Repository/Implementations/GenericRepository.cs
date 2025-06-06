using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MySociety.Entity.Data;
using MySociety.Repository.Interfaces;

namespace MySociety.Repository.Implementations;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    private readonly MySocietyDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(MySocietyDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    #region C : Create
    /*------------------------------adds a new entity (record) to the database----------------------------------------
    -------------------------------------------------------------------------------------------------------*/
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<int> AddAsyncReturnId(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        return typeof(T).GetProperty("Id")?.GetValue(entity) is int id ? id : 0;
    }
    #endregion C : Create

    #region R : Read

    /*----------------------retrieves a single record from the database by its primary key (id)----------------------------------------
    -------------------------------------------------------------------------------------------------------*/
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /*----------------------fetches a single record from the database based on a given condition----------------------------------------
    -------------------------------------------------------------------------------------------------------*/
    public async Task<T?> GetByStringAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    /*----------------------------To Get the all the values from table----------------------------------------
    -------------------------------------------------------------------------------------------------------*/
    public IEnumerable<T> GetAll() => _dbSet;

    public async Task<IEnumerable<T>> GetByCondition(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        List<Expression<Func<T, object>>>? includes = null,
        List<Func<IQueryable<T>, IQueryable<T>>>? thenIncludes = null)
    {

        IQueryable<T> query = _dbSet;

        //Apply Filters
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        //Order By
        if (orderBy != null)
        {
            query = orderBy(query);
        }

        // Apply Includes (First-level navigation properties)
        if (includes != null)
        {
            foreach (Expression<Func<T, object>>? include in includes)
            {
                query = query.Include(include);
            }
        }

        // Apply ThenIncludes (Deeper navigation properties)
        if (thenIncludes != null)
        {
            foreach (var thenInclude in thenIncludes)
            {
                query = thenInclude(query);
            }
        }

        return await query.ToListAsync();
    }

    public (IEnumerable<T> items, int totalCount) GetPagedRecords
    (
        int pageSize,
        int pageNumber,
        IEnumerable<T> items
    )
    {
        int totalCount = items.Count();

        items = items
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }

    #endregion R : Read

    #region U : Update
    /*------------------------------updates an existing entity in the database----------------------------------------
    -------------------------------------------------------------------------------------------------------*/
    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    #endregion U : Update

    #region D : Delete
    /*------------------Only Soft Delete is allowed so no remove method------------------------------------
    -------------------------------------------------------------------------------------------------------*/
    #endregion D : Delete

}
