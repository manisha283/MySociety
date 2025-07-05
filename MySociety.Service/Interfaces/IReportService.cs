namespace MySociety.Service.Interfaces;

public interface IReportService
{
    Task<byte[]> RenderReportAsync(string reportPath, string format, Dictionary<string, string> parameters);
}
