using LightOffSchedule.Repository.Models;

namespace LightOffSchedule.Repository;

public interface ILightOffScheduleRepository
{
    Task CreateManyAsync(IEnumerable<LightOffScheduleModel> models, CancellationToken cancellationToken = default);

    Task DeleteAllAsync(CancellationToken cancellationToken = default);
    
    Task DeleteIntervalAsync(LightOffScheduleIntervalModel model, CancellationToken cancellationToken = default);

    Task EditGroupAsync(LightOffScheduleModel model, CancellationToken cancellationToken = default);

    Task<LightOffScheduleModel?> GetByGroupAsync(int group, CancellationToken cancellationToken = default);

    Task<IEnumerable<LightOffScheduleModel>> GetAllAsync(CancellationToken cancellationToken = default);
}