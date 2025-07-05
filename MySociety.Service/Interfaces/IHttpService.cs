namespace MySociety.Service.Interfaces;

public interface IHttpService
{
    Task<int> LoggedInUserId();
    Task<string> LoggedInUserName();
    string LoggedInUserRole();
}
