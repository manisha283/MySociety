using MySociety.Entity.Models;

namespace MySociety.Entity.ViewModels;

public class VisitorIndexVM
{
    public List<VisitPurpose> VisitPurposes { get; set; } = new List<VisitPurpose>();
}
