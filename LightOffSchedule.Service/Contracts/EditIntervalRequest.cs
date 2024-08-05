namespace LightOffSchedule.Service.Contracts;

public class EditIntervalRequest
{
    public int Group { get; set; }
    
    public TimeSpan Start { get; set; }
    
    public TimeSpan End { get; set; }
    
    public TimeSpan NewStart { get; set; }
    
    public TimeSpan NewEnd { get; set; }
}