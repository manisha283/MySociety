namespace MySociety.Service.Interfaces;

public interface IHttpService
{
    Task<int> LoggedInUserId();
    string LoggedInUserRole();
}
