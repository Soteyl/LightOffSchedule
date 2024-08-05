using LightOffSchedule.Repository.Models;

namespace LightOffSchedule.Models;

public class HomeViewModel: BaseViewModel
{
    public List<LightOffScheduleModel> LightOffSchedules { get; set; } = new();

    public List<LightOffScheduleModel> NowLightOffSchedules { get; set; } = new();
}