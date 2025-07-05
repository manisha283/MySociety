using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MySociety.Entity.Data;
using MySociety.Entity.HelperModels;
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

    /*----------------------retrieves a single record from the database by its primary key (id)----------------------------------------
    -------------------------------------------------------------------------------------------------------*/
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /*----------------------fetches a single record from the database based on a given condition----------------------------------------
    -------------------------------------------------------------------------------------------------------*/
    public async Task<T?> GetByStringAsync
    (
        Expression<Func<T, bool>> predicate,
        List<Func<IQueryable<T>, IQueryable<T>>>? queries = null,
         Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool firstRecord = true
    )
    {
        IQueryable<T> records = _dbSet;

        if (queries != null)
        {
            foreach (Func<IQueryable<T>, IQueryable<T>>? query in queries)
            {
                records = query(records);
            }
        }

        if (orderBy != null)
        {
            records = orderBy(records);
        }

        if (firstRecord)
        {
            return await records.FirstOrDefaultAsync(predicate);
        }
        else
        {
            return await records.LastOrDefaultAsync(predicate);
        }
    }

    /*----------------------------To Get the all the values from table----------------------------------------
    -------------------------------------------------------------------------------------------------------*/
    public IEnumerable<T> GetAll() => _dbSet;

    /*----------------------------To Get records based on the condition ----------------------------------------
    -------------------------------------------------------------------------------------------------------*/
    public async Task<DbResult<T>> GetRecords(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        List<Expression<Func<T, object>>>? includes = null,
        List<Func<IQueryable<T>, IQueryable<T>>>? queries = null,
        List<Expression<Func<T, bool>>>? predicates = null,
        int pageSize = 0,
        int pageNumber = 0)
    {
        DbResult<T> result = new();

        IQueryable<T> records = _dbSet;

        //Apply Filters
        if (predicate != null)
        {
            records = records.Where(predicate);
        }

        //Order By
        if (orderBy != null)
        {
            records = orderBy(records);
        }

        // Apply Includes (First-level navigation properties)
        if (includes != null)
        {
            foreach (Expression<Func<T, object>>? include in includes)
            {
                records = records.Include(include);
            }
        }

        // Apply any query like ThenIncludes for Deeper navigation properties
        if (queries != null)
        {
            foreach (Func<IQueryable<T>, IQueryable<T>>? query in queries)
            {
                records = query(records);
            }
        }

        //Apply multiple predicates
        if (predicates != null)
        {
            foreach (var p in predicates)
            {
                records = records.Where(p);
            }
        }

        //Check whether pagination is required
        if (pageSize == 0)
        {
            result.Records = await records.ToListAsync();
        }
        else
        {
            result.TotalRecord = records.Count();
            result.Records = await records.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        return result;
    }

    /*------------------------------updates an existing entity in the database----------------------------------------
    -------------------------------------------------------------------------------------------------------*/
    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public int GetCount(Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> records = _dbSet;

        //Apply Filters
        if (predicate != null)
        {
            records = records.Where(predicate);
        }

        return records.Count();
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
        await _context.SaveChangesAsync();
    }
}
