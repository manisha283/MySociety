using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using AspNetCore.Reporting.ReportExecutionService;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class ReportService : IReportService
{
    private readonly ReportExecutionServiceSoapClient _client;
    public ReportService()
    {
        BasicHttpBinding binding = new(BasicHttpSecurityMode.TransportCredentialOnly)
        {
            MaxReceivedMessageSize = int.MaxValue,
            Security = { Transport = { ClientCredentialType = HttpClientCredentialType.Windows } }
        };

        EndpointAddress endpoint = new("http://localhost/ReportExecution2005.asmx");

        _client = new ReportExecutionServiceSoapClient(binding, endpoint);

        _client.ClientCredentials.Windows.ClientCredential = new NetworkCredential("username", "password", "domain");
    }

    public async Task<byte[]> RenderReportAsync(string reportPath, string format, Dictionary<string, string> parameters)
    {
        // Create and send LoadReportRequest
        LoadReportRequest loadRequest = new()
        {
            Report = reportPath,
            HistoryID = null
        };

        LoadReportResponse loadResponse = await _client.LoadReportAsync(loadRequest);

        // Prepare parameters
        ParameterValue[]? paramList = parameters.Select(p => new ParameterValue
        {
            Name = p.Key,
            Value = p.Value
        }).ToArray();

        // Set parameters with request object
        SetExecutionParametersRequest paramRequest = new()
        {
            Parameters = paramList,
            ParameterLanguage = "en-us"
        };

        await _client.SetExecutionParametersAsync(paramRequest);

        // Render report with request object
        RenderRequest renderRequest = new RenderRequest
        {
            Format = format,
            DeviceInfo = null
        };

        RenderResponse renderResponse = await _client.RenderAsync(renderRequest);

        return renderResponse.Result;
    }

}
