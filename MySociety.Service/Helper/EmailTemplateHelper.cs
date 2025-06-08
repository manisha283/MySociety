using MySociety.Entity.ViewModels;

namespace MySociety.Service.Helper;

public static class EmailTemplateHelper
{
    private static string GetTemplateContent(string templateName)
    {
        // Navigate to the correct folder relative to your project root
        // var baseDirectory = AppContext.BaseDirectory;
        // var templatePath = Path.Combine(baseDirectory, "..", "..", "..", "MySociety.Service", "EmailTemplates", $"{templateName}.html");
        string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates", $"{templateName}.html");
        // string templatePath = Path.Combine(@"M:\Tatvasoft\MySociety\MySociety.Service\EmailTemplates", $"{templateName}.html");
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Email template {templateName}.html not found at {templatePath}");
        }
        return File.ReadAllText(templatePath);
    }

    public static string ResetPassword(string resetLink)
    {
        string template = GetTemplateContent("ResetPassword");
        return template.Replace("{resetLink}", resetLink);
    }

    public static string NewPassword(string password)
    {
        string template = GetTemplateContent("NewPassword");
        return template.Replace("{password}", password);
    }

    public static string NewUserRegistration(RegisterVM registerVM)
    {
        string template = GetTemplateContent("NewUserRegistration");
        return template.Replace("{name}", registerVM.Name)
                      .Replace("{email}", registerVM.Email)
                      .Replace("{blockName}", registerVM.BlockName)
                      .Replace("{floorName}", registerVM.FloorName)
                      .Replace("{houseName}", registerVM.HouseName)
                      .Replace("{registeredOn}", DateTime.Now.ToString());
    }

    public static string AdminApprovalNotification(string name, string loginUrl)
    {
        string template = GetTemplateContent("AdminApprovalNotification");
        return template.Replace("{name}", name)
                      .Replace("{loginUrl}", loginUrl);
    }

    public static string OtpVerification(string otp)
    {
        string template = GetTemplateContent("OtpVerification");
        return template.Replace("{otp}", otp);
    }
}

