namespace LightOffSchedule.Repository.Models;

public class LightOffScheduleIntervalModel
{
    public TimeSpan Start { get; set; }
    
    public TimeSpan End { get; set; }
    
    public int GroupNumber { get; set; }
}