namespace LightOffSchedule.Service.Contracts;

public class AddIntervalRequest
{
    public int Group { get; set; }
    
    public TimeSpan Start { get; set; }
    
    public TimeSpan End { get; set; }
}