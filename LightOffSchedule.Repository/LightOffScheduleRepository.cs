using LightOffSchedule.Data;
using LightOffSchedule.Data.Entities;
using LightOffSchedule.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace LightOffSchedule.Repository;

public class LightOffScheduleRepository(IDbContextFactory<LightOffScheduleContext> contextFactory): ILightOffScheduleRepository
{
    public async Task CreateManyAsync(IEnumerable<LightOffScheduleModel> models, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        context.LightOffSchedules.AddRange(models.Select(x => new LightOffScheduleEntity()
        {
            GroupNumber = x.GroupNumber,
            Intervals = x.Intervals?.Select(i => new LightOffScheduleIntervalEntity()
            {
                LightOffScheduleEntityGroupNumber = x.GroupNumber,
                End = i.End,
                Start = i.Start
            }).ToList()!
        }));

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAllAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        await context.LightOffSchedules.ExecuteDeleteAsync(cancellationToken: cancellationToken);
    }

    public async Task DeleteIntervalAsync(LightOffScheduleIntervalModel model, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var entity = await context.LightOffScheduleIntervals.AsNoTracking().FirstOrDefaultAsync(x =>
            x.Start.Equals(model.Start) 
            && x.End.Equals(model.End) 
            && x.LightOffScheduleEntityGroupNumber.Equals(model.GroupNumber), cancellationToken);

        if (entity is not null)
        {
             context.Remove(entity);
             await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task EditGroupAsync(LightOffScheduleModel model,
        CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var entity = await context.LightOffSchedules.Include(x => x.Intervals).FirstOrDefaultAsync(x => x.GroupNumber.Equals(model.GroupNumber),
            cancellationToken);

        if (entity is null)
        {
            entity = new LightOffScheduleEntity() { GroupNumber = model.GroupNumber };
            context.Add(entity);
        }

        context.RemoveRange(entity.Intervals);

        entity.Intervals = model.Intervals?.Select(x => new LightOffScheduleIntervalEntity()
        {
            Start = x.Start,
            End = x.End,
            LightOffScheduleEntityGroupNumber = x.GroupNumber
        }).ToList() ?? [];

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<LightOffScheduleModel?> GetByGroupAsync(int group, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var existingEntity = await context.LightOffSchedules
                                    .AsNoTracking()
                                    .Include(x => x.Intervals)
                                    .FirstOrDefaultAsync(x => x.GroupNumber.Equals(group), 
                                        cancellationToken: cancellationToken);
        
        return existingEntity == null ? null : new LightOffScheduleModel()
        {
            GroupNumber = existingEntity.GroupNumber,
            Intervals = existingEntity.Intervals.Select(x => new LightOffScheduleIntervalModel()
            {
                End = x.End,
                Start = x.Start
            }).ToList()
        };
    }
    
    public async Task<IEnumerable<LightOffScheduleModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.LightOffSchedules
                      .AsNoTracking()
                      .Include(x => x.Intervals)
                      .Select(x => new LightOffScheduleModel()
                      {
                          GroupNumber = x.GroupNumber,
                          Intervals = x.Intervals.Select(i => new LightOffScheduleIntervalModel()
                          {
                              End = i.End,
                              Start = i.Start,
                              GroupNumber = i.LightOffScheduleEntityGroupNumber
                          })
                      }).ToListAsync(cancellationToken);
    }
}