using LightOffSchedule.Common;
using LightOffSchedule.Repository;
using LightOffSchedule.Repository.Models;
using LightOffSchedule.Service.Common.TextParse;
using LightOffSchedule.Service.Contracts;

namespace LightOffSchedule.Service;

public class LightOffScheduleService(ILightOffScheduleRepository repository): ILightOffScheduleService
{
    public async Task<ServiceResponse> UpdateLightOffScheduleByFileAsync(UpdateLightOffScheduleByFileRequest request, 
        CancellationToken cancellationToken = default)
    {
        var validation = await new UpdateLightOffScheduleByFileRequestValidator().ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return ServiceResponse.Error(validation);
        
        var parseDataResult = new LightOffScheduleFromTextParser().Parse(request.FileText);

        if (!parseDataResult.IsSuccess)
            return parseDataResult;

        var withoutIntersections = MergeIntersections(parseDataResult.Result!);
        withoutIntersections.ForEach(x =>
        {
            if (x.Intervals?.Any() is not true) return;

            foreach (var interval in x.Intervals)
            {
                if (interval.Start > interval.End)
                    (interval.Start, interval.End) = (interval.End, interval.Start);
            }
        });

        await repository.DeleteAllAsync(cancellationToken);
        await repository.CreateManyAsync(withoutIntersections, cancellationToken);
        
        return ServiceResponse.Success();
    }

    public async Task<ServiceResponse<IEnumerable<LightOffScheduleModel>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return ServiceResponse<IEnumerable<LightOffScheduleModel>>.Success(await repository.GetAllAsync(cancellationToken));
    }

    public async Task<ServiceResponse<LightOffScheduleModel?>> GetByGroupAsync(GetByGroupRequest request, CancellationToken cancellationToken = default)
    {
        var schedule = await repository.GetByGroupAsync(request.Group, cancellationToken);

        if (schedule == null)
            return ServiceResponse<LightOffScheduleModel?>.Error(new Error("Черга не знайдена"));

        return ServiceResponse<LightOffScheduleModel?>.Success(schedule);
    }

    public async Task<ServiceResponse> DeleteIntervalAsync(DeleteIntervalRequest request, CancellationToken cancellationToken = default)
    {
        await repository.DeleteIntervalAsync(new LightOffScheduleIntervalModel()
        {
            GroupNumber = request.Group,
            Start = request.Start,
            End = request.End
        }, cancellationToken);
        return ServiceResponse.Success();
    }

    public async Task<ServiceResponse> EditIntervalAsync(EditIntervalRequest request, CancellationToken cancellationToken = default)
    {
        var group = await repository.GetByGroupAsync(request.Group, cancellationToken);
        group ??= new LightOffScheduleModel();
        group.Intervals ??= new List<LightOffScheduleIntervalModel>();
        var intervalToEdit = group.Intervals.FirstOrDefault(x => x.Start.Equals(request.Start) && x.End.Equals(request.End)) 
                             ?? new LightOffScheduleIntervalModel() { GroupNumber = request.Group };

        intervalToEdit.Start = request.NewStart;
        intervalToEdit.End = request.NewEnd;
        
        group!.Intervals = MergeIntersection(group.Intervals ?? []);
        await repository.EditGroupAsync(group, cancellationToken);
        return ServiceResponse.Success();
    }

    public async Task<ServiceResponse> AddIntervalAsync(AddIntervalRequest request, CancellationToken cancellationToken = default)
    {
        var group = await repository.GetByGroupAsync(request.Group, cancellationToken);
        group ??= new LightOffScheduleModel();
        group.Intervals ??= new List<LightOffScheduleIntervalModel>();

        group.Intervals = group.Intervals.Append(new LightOffScheduleIntervalModel()
        {
            Start = request.Start,
            End = request.End,
            GroupNumber = request.Group
        });
        
        group!.Intervals = MergeIntersection(group.Intervals ?? []);
        await repository.EditGroupAsync(group, cancellationToken);
        return ServiceResponse.Success();
    }

    private List<LightOffScheduleModel> MergeIntersections(IEnumerable<LightOffScheduleModel> models)
    {
        return models.GroupBy(x => x.GroupNumber).Select(x => new LightOffScheduleModel()
        {
            GroupNumber = x.Key,
            Intervals = MergeIntersection(x.Select(t => t.Intervals).SelectMany(t => t!))
        }).ToList();
    }
    
    private IEnumerable<LightOffScheduleIntervalModel> MergeIntersection(IEnumerable<LightOffScheduleIntervalModel> intervals)
    {
        var mergedIntervals = new List<LightOffScheduleIntervalModel>();
        intervals = intervals.OrderBy(t => t.Start).ToList();
        var current = intervals.First();

        foreach (var interval in intervals.Skip(1))
        {
            if (interval.Start <= current.End)
            {
                current.End = interval.End > current.End ? interval.End : current.End;
            }
            else
            {
                mergedIntervals.Add(current);
                current = interval;
            }
        }
        mergedIntervals.Add(current);

        return mergedIntervals;
    }
}