namespace Cenace.API.Domain.Entities;

public class CapacityDemand
{
    public int Id { get; set; }
    public string Zone { get; set; }
    public string Participant { get; set; }
    public string Subaccount { get; set; }
    public double DemandCapacityMW { get; set; }
    public double AnnualPowerRequirementMW { get; set; }
    public double EfficientAnnualRequirementMW { get; set; }
    public DateTime CreatedAt { get; set; }
}
