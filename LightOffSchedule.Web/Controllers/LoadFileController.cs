using LightOffSchedule.Models;
using LightOffSchedule.Service;
using LightOffSchedule.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LightOffSchedule.Controllers;

public class LoadFileController(ILightOffScheduleService service): Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> LoadFileAsync(LoadFileViewModel viewModel, CancellationToken cancellationToken = default)
    {
        using var reader = new StreamReader(viewModel.File.OpenReadStream());

        var result = await service.UpdateLightOffScheduleByFileAsync(new UpdateLightOffScheduleByFileRequest()
        {
            FileText = await reader.ReadToEndAsync(cancellationToken)
        }, cancellationToken);

        if (!result.IsSuccess)
        {
            foreach (var error in result.Errors!)
            {
                ModelState.AddModelError("File", error.ErrorMessage);
            }
            return View("Index");
        }
        
        return RedirectToAction("Index", "Home");
    }
}