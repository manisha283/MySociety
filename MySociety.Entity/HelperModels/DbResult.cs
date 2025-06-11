namespace MySociety.Entity.HelperModels;

public class DbResult<T> where T : class
{
    public IEnumerable<T> Records { get; set; } = Enumerable.Empty<T>();
    public int TotalRecord { get; set; } = 0;
}
