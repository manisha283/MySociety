namespace MySociety.Service.Common;

public class NotificationMessages
{
    //Generic CRUD Success
    public const string Added = "{0} has been added successfully!";
    public const string Updated = "{0} has been updated successfully!";
    public const string Deleted = "{0} has been deleted successfully!";


    public const string TryAgain = " Please try again!";
    public const string AlreadyExisted = "{0} already existed!";
    public const string NotFound = "{0} not found!" + TryAgain;
    public const string Invalid = "Invalid {0}!" + TryAgain;
    public const string Successfully = "{0} Sucessfully!";
    public const string Failed = "{0} Failed!";
    public const string ModelStateInvalid = "Details are Invalid!";


    public const string LinkExpired = "Link expired!" + TryAgain;
    public const string AlreadyUsed = "Link already used to reset password!";
    public const string EmailSent = "Email has been sent successfully!";
    public const string EmailSendingFailed = "Failed to send the email." + TryAgain;
    public const string UserNotActive = "User is not an Active member of society";
    public const string UserRegistered = "Registration done successfully! Wait for the admin approval.";
    public const string UserRejected = "Your request was not approved. For more details, please contact the Admin.";
    public const string UserPending = "Your request is under review. You will be notified once it is approved.";
    public const string Approved = "{0} has been approved successfully!";
    public const string Rejected = "{0} has been rejected successfully!";
    public const string VisitorCheckedOut = "The visitor has checked out successfully!";

    
}
