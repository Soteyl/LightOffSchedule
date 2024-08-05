using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightOffSchedule.Data.Entities;

public class LightOffScheduleEntity
{
    public int GroupNumber { get; set; }

    public List<LightOffScheduleIntervalEntity> Intervals { get; set; } = new ();
}

public class LightOffScheduleEntityConfiguration: IEntityTypeConfiguration<LightOffScheduleEntity>
{
    public void Configure(EntityTypeBuilder<LightOffScheduleEntity> builder)
    {
        builder.HasKey(x => x.GroupNumber);
        builder.Property(x => x.GroupNumber).ValueGeneratedNever();
        builder.HasMany(x => x.Intervals)
               .WithOne(x => x.LightOffScheduleEntity)
               .HasForeignKey(x => x.LightOffScheduleEntityGroupNumber)
               .OnDelete(DeleteBehavior.Cascade);
    }
}