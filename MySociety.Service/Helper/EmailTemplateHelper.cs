using MySociety.Entity.ViewModels;
using MySociety.Service.Common;

namespace MySociety.Service.Helper;

public static class EmailTemplateHelper
{
    public static string GetTemplateContent(string templateName)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "EmailTemplates", $"{templateName}.html");
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Email template {templateName}.html not found at {filePath}");
        }
        string baseTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "EmailTemplates", "BaseTemplate.html");
        var baseTemplate = File.ReadAllText(baseTemplatePath);

        return baseTemplate.Replace("{content}", File.ReadAllText(filePath));

        // return File.ReadAllText(filePath);
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

    public static string AdminApprovalNotification(string name)
    {
        string template = GetTemplateContent("AdminApprovalNotification");
        return template.Replace("{name}", name)
                      .Replace("{loginUrl}", CommonUrls.LoginUrl);
    }

    public static string OtpVerification(string otp)
   {
        string template = GetTemplateContent("OtpVerification");
        return template.Replace("{otp}", otp);
    }
}
