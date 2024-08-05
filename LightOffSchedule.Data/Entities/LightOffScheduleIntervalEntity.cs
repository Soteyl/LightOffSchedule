using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightOffSchedule.Data.Entities;

public class LightOffScheduleIntervalEntity
{
    public TimeSpan Start { get; set; }
    
    public TimeSpan End { get; set; }
    
    public int LightOffScheduleEntityGroupNumber { get; set; }
    
    public LightOffScheduleEntity LightOffScheduleEntity { get; set; }
}

public class LightOffScheduleIntervalEntityConfiguration: IEntityTypeConfiguration<LightOffScheduleIntervalEntity>
{
    public void Configure(EntityTypeBuilder<LightOffScheduleIntervalEntity> builder)
    {
        builder.HasKey(x => new {x.Start, x.End, x.LightOffScheduleEntityGroupNumber});
    }
}