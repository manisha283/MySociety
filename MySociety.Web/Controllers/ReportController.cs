using Microsoft.AspNetCore.Mvc;
using MySociety.Service.Interfaces;

namespace MySociety.Web.Controllers;

public class ReportController : Controller
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    public IActionResult Index()
    {
        ViewData["sidebar-active"] = "Report";
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DownloadReport(string format = "PDF")
    {
        // var reportUrl = "http://localhost/ReportServer?/FolderName/VisitorReport&rs:Format=" + format;
        var reportUrl = "http://localhost:5112/files/Visitors.pdf";
        using var httpClient = new HttpClient();
        var reportBytes = await httpClient.GetByteArrayAsync(reportUrl);

        string contentType = format.ToUpper() switch
        {
            "PDF" => "application/pdf",
            "EXCEL" => "application/vnd.ms-excel",
            "WORD" => "application/msword",
            _ => "application/octet-stream"
        };

        return File(reportBytes, contentType, $"Report.{format.ToLower()}");
    }

    [HttpGet]
    public async Task<IActionResult> DownloadVisitorReport(string format = "PDF", string status = "Approved")
    {
        Dictionary<string, string> parameters = new()
        {
            { "VisitorStatusParam", status },
        };

        string reportPath = "/Reports/VisitorReport";

        byte[] reportBytes = await _reportService.RenderReportAsync(reportPath, format, parameters);

        string contentType = format.ToUpper() switch
        {
            "PDF" => "application/pdf",
            "EXCEL" => "application/vnd.ms-excel",
            "WORD" => "application/msword",
            _ => "application/octet-stream"
        };

        return File(reportBytes, contentType, $"VisitorReport.{format.ToLower()}");
    }

}
