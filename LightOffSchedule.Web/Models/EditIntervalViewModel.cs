namespace LightOffSchedule.Models;

public class EditIntervalViewModel: BaseViewModel
{
    public int LightOffScheduleGroupNumber { get; set; }
    
    public TimeSpan OldStart { get; set; }
    
    public TimeSpan OldEnd { get; set; }
    
    public TimeSpan NewStart { get; set; }
    
    public TimeSpan NewEnd { get; set; }
}