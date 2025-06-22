namespace MySociety.Service.Interfaces;

public interface IVisitorFeedbackService
{
    Task Add(int visitorId, int rating, string feedback);
}
