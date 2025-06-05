using Microsoft.Extensions.Configuration;

namespace MySociety.Service.Configuration;

public class EmailConfig
{
     public static string Host { get; set; } = "mail.etatvasoft.com";
    public static int Port { get; set; } = 587; 
    public static string UserName { get; set; } = "test.dotnet@etatvasoft.com";
    public static string Password { get; set; } = "P}N^{z-]7Ilp";
    public static string FromEmail { get; set; } = "test.dotnet@etatvasoft.com";
    public static string FromName { get; set; } = "MySociety";

    public static void LoadEmailConfiguration(IConfiguration configuration)
    {
        Host = configuration["SmtpSettings:Host"] ?? Host;
        UserName = configuration["SmtpSettings:UserName"] ?? UserName;
        Password = configuration["SmtpSettings:Password"] ?? Password;
        FromEmail = configuration["SmtpSettings:FromEmail"] ?? FromEmail;

        if(int.TryParse(configuration["SmtpSettings:Port"], out int port))
        {
            Port = port;
        }
    }
}
