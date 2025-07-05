using MySociety.Entity.ViewModels;

namespace MySociety.Entity.HelperModels;

public class PaginatedRecords<T> where T : class
{
    public IEnumerable<T> Records { get; set; } = Enumerable.Empty<T>();
    public int TotalRecord { get; set; } = 0;
    public Pagination Page { get; set; } = new();
}