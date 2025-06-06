using MySociety.Entity.ViewModels;

namespace MySociety.Service.Helper;

public static class EmailTemplateHelper
{
    public static string EmailTemplate = @"
                <div style='background-image: linear-gradient(to top, #fff1eb 0%, #bbe5f8 100%);'>
                    <div style='background-color: #3a6073; color: white; height: 90px; font-size: 40px; font-weight: 600; text-align: center; padding-top: 40px; margin-bottom: 0px;'>MySociety</div>
                    <div style='font-family:Verdana, Geneva, Tahoma, sans-serif; margin-top: 0px; font-size: 20px; padding: 10px;'>
                        {0}
                    </div>
                </div>";

    public static string ResetPassword(string resetLink)
    {
        string body = $@"<p>My Society,</p>
                        <p>Please click <a href='{resetLink}'>here</a> to reset your account password.</p>
                        <p>If you have any issues, please contact support.</p>
                        <p><span style='color: orange;'>Important:</span> The link expires in 24 hours.</p>";
        body = EmailTemplate.Replace("{0}", body);
        return body;
    }

    public static string NewPassword(string password)
    {
        string body = $@"<p>My Society,</p>
                        <h3>Your Password is : {password}</h3>
                        <p>If you encounter any issues or have any question, please do not hesitate to contact our support team.</p>";
        return EmailTemplate.Replace("{0}", body);
    }

    public static string NewUserRegistration(RegisterVM registerVM)
    {
        string body = $@"<p>My Society - New User Registration</p>
                     <p>A new user has registered on the system. Below are the details:</p>
                     <ul>
                        <li><strong>Name:</strong> {registerVM.Name}</li>
                        <li><strong>Email:</strong> {registerVM.Email}</li>
                        <li><strong>Block:</strong> {registerVM.BlockName}</li>
                        <li><strong>Floor:</strong> {registerVM.FloorName}</li>
                        <li><strong>House:</strong> {registerVM.HouseName}</li>
                        <li><strong>Registered On:</strong> {DateTime.Now}</li>
                     </ul>
                     <p>Please log in to the admin panel if you wish to take any further action.</p>";
        body = EmailTemplate.Replace("{0}", body);
        return body;
    }

}
