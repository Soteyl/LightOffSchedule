using LightOffSchedule.Common;
using LightOffSchedule.Repository.Models;
using LightOffSchedule.Service.Contracts;

namespace LightOffSchedule.Service;

public interface ILightOffScheduleService
{
    Task<ServiceResponse> UpdateLightOffScheduleByFileAsync(UpdateLightOffScheduleByFileRequest request,
        CancellationToken cancellationToken = default);

    Task<ServiceResponse<IEnumerable<LightOffScheduleModel>>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<ServiceResponse<LightOffScheduleModel?>> GetByGroupAsync(GetByGroupRequest request, CancellationToken cancellationToken = default);

    Task<ServiceResponse> DeleteIntervalAsync(DeleteIntervalRequest request, CancellationToken cancellationToken = default);
    
    Task<ServiceResponse> EditIntervalAsync(EditIntervalRequest request, CancellationToken cancellationToken = default);
    
    Task<ServiceResponse> AddIntervalAsync(AddIntervalRequest request, CancellationToken cancellationToken = default);
}