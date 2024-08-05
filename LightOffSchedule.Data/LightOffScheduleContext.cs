using LightOffSchedule.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LightOffSchedule.Data;

public class LightOffScheduleContext(DbContextOptions<LightOffScheduleContext> options): DbContext(options)
{
    public DbSet<LightOffScheduleEntity> LightOffSchedules { get; set; }
    
    public DbSet<LightOffScheduleIntervalEntity> LightOffScheduleIntervals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LightOffScheduleEntityConfiguration());
        modelBuilder.ApplyConfiguration(new LightOffScheduleIntervalEntityConfiguration());
    }
}

public class LightOffScheduleContextFactory: IDesignTimeDbContextFactory<LightOffScheduleContext>
{
    public LightOffScheduleContext CreateDbContext(string[] args)
    {
        return new LightOffScheduleContext(new DbContextOptionsBuilder<LightOffScheduleContext>().UseSqlite().Options);
    }
}