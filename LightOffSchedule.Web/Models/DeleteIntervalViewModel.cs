namespace LightOffSchedule.Models;

public class DeleteIntervalViewModel: BaseViewModel
{
    public int LightOffScheduleGroupNumber { get; set; }
    
    public TimeSpan Start { get; set; }
    
    public TimeSpan End { get; set; }
}