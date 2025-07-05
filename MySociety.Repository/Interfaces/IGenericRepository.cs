using System.Linq.Expressions;
using MySociety.Entity.HelperModels;

namespace MySociety.Repository.Interfaces;

public interface IGenericRepository<T>
    where T : class
{
    Task AddAsync(T entity);
    Task<int> AddAsyncReturnId(T entity);

    IEnumerable<T> GetAll();

    Task<DbResult<T>> GetRecords
    (
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        List<Expression<Func<T, object>>>? includes = null,
        List<Func<IQueryable<T>, IQueryable<T>>>? queries = null,
        List<Expression<Func<T, bool>>>? predicates = null,
        int pageSize = 0,
        int pageNumber = 0);

    Task<T?> GetByIdAsync(int id);

    Task<T?> GetByStringAsync
    (
        Expression<Func<T, bool>> predicate,
        List<Func<IQueryable<T>, IQueryable<T>>>? queries = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool firstRecord = true
    );

    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entities);

    int GetCount(Expression<Func<T, bool>>? predicate = null);
}
