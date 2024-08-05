namespace LightOffSchedule.Repository.Models;

public class LightOffScheduleModel
{
    public int GroupNumber { get; set; }

    public IEnumerable<LightOffScheduleIntervalModel>? Intervals { get; set; }
}