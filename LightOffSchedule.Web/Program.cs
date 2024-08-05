using LightOffSchedule.Data;
using LightOffSchedule.Repository;
using LightOffSchedule.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("secrets.json")
                    .Build();

builder.Services.AddDbContextFactory<LightOffScheduleContext>(o => o.UseSqlite(configuration.GetConnectionString("sqlite")));
builder.Services.AddSingleton<ILightOffScheduleRepository, LightOffScheduleRepository>();
builder.Services.AddSingleton<ILightOffScheduleService, LightOffScheduleService>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.Services.GetRequiredService<IDbContextFactory<LightOffScheduleContext>>().CreateDbContext().Database.Migrate();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();