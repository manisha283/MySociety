using MySociety.Entity.Models;
using MySociety.Repository.Interfaces;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class VisitorFeedbackService : IVisitorFeedbackService
{
    private readonly IGenericRepository<VisitorFeedback> _feedbackRepository;

    public VisitorFeedbackService(IGenericRepository<VisitorFeedback> feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    public async Task Add(int visitorId, int rating, string feedback)
    {
        VisitorFeedback visitorFeedback = new()
        {
            VisitorId = visitorId
        };

        if (rating != 0)        
        {
            visitorFeedback.Rating = rating;
        }

        if (!string.IsNullOrEmpty(feedback))
        {
            visitorFeedback.Feedback = feedback;
        }

        await _feedbackRepository.AddAsync(visitorFeedback);
    }

}
