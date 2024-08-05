namespace LightOffSchedule.Models;

public class AddIntervalViewModel: BaseViewModel
{
    public int LightOffScheduleGroupNumber { get; set; }
    
    public TimeSpan Start { get; set; }
    
    public TimeSpan End { get; set; }
}