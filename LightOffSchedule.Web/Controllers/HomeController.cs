using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using LightOffSchedule.Models;
using LightOffSchedule.Repository.Models;
using LightOffSchedule.Service;
using LightOffSchedule.Service.Contracts;
using Newtonsoft.Json;

namespace LightOffSchedule.Controllers;

public class HomeController(ILightOffScheduleService service): Controller
{
    public async Task<IActionResult> IndexAsync(CancellationToken cancellationToken = default)
    {
        var schedules = ((await service.GetAllAsync(cancellationToken)).Result ?? []).ToList();
        return View(new HomeViewModel()
        {
            LightOffSchedules = schedules,
            NowLightOffSchedules = schedules.Select(x => new LightOffScheduleModel()
            {
                GroupNumber = x.GroupNumber,
                Intervals = x.Intervals?.Where(interval => 
                    interval.Start < DateTime.Now.TimeOfDay && interval.End > DateTime.Now.TimeOfDay).ToList()
            }).Where(x => x.Intervals?.Any() is true).ToList()
        });
    }

    public async Task<IActionResult> DownloadJsonAsync(BaseViewModel viewModel, CancellationToken cancellationToken = default)
    {
        IEnumerable<LightOffScheduleModel> schedules;

        if (viewModel.SearchGroup.HasValue && (await service.GetByGroupAsync(new GetByGroupRequest() { Group = viewModel.SearchGroup.Value }, cancellationToken)).Result 
            is not null and var schedule)
        {
            schedules = [schedule];
        }
        else
        {
            schedules = (await service.GetAllAsync(cancellationToken)).Result ?? [];
        }
        
        return File(new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(schedules, Formatting.Indented))), 
            "application/octet-stream", "lightOffSchedule.json");
    }

    [HttpGet]
    public async Task<IActionResult> GetByGroupAsync(BaseViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var schedule = await service.GetByGroupAsync(new GetByGroupRequest() { Group = viewModel.SearchGroup!.Value }, cancellationToken);

        if (!schedule.IsSuccess)
        {
            foreach (var error in schedule.Errors!)
            {
                ModelState.AddModelError("SearchGroup", error.ErrorMessage);
            }
            return View("Index", new HomeViewModel() { });
        }

        return View("Index", new HomeViewModel()
        {
            LightOffSchedules = new List<LightOffScheduleModel>()
            {
                schedule.Result!
            }
        });
    }

    [HttpGet]
    public IActionResult EditIntervalPage(EditIntervalViewModel viewModel)
    {
        return View(viewModel);
    }
    
    [HttpGet]
    public IActionResult AddIntervalPage(AddIntervalViewModel viewModel)
    {
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditIntervalAsync(EditIntervalViewModel viewModel)
    {
        await service.EditIntervalAsync(new EditIntervalRequest()
        {
            Start = viewModel.OldStart,
            End = viewModel.OldEnd,
            NewStart = viewModel.NewStart,
            NewEnd = viewModel.NewEnd,
            Group = viewModel.LightOffScheduleGroupNumber
        });
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public async Task<IActionResult> AddIntervalAsync(AddIntervalViewModel viewModel, CancellationToken cancellationToken = default)
    {
        await service.AddIntervalAsync(new AddIntervalRequest()
        {
            Start = viewModel.Start,
            End = viewModel.End,
            Group = viewModel.LightOffScheduleGroupNumber
        }, cancellationToken);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteIntervalAsync(DeleteIntervalViewModel viewModel, CancellationToken cancellationToken = default)
    {
        await service.DeleteIntervalAsync(new DeleteIntervalRequest()
        {
            Start = viewModel.Start,
            End = viewModel.End,
            Group = viewModel.LightOffScheduleGroupNumber
        }, cancellationToken);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}