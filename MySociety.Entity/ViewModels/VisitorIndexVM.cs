using MySociety.Entity.Models;

namespace MySociety.Entity.ViewModels;

public class VisitorIndexVM
{
    public List<VisitPurpose> VisitPurposes { get; set; } = new List<VisitPurpose>();
    public List<VisitorStatus> VisitorStatuses { get; set; } = new List<VisitorStatus>();
}
