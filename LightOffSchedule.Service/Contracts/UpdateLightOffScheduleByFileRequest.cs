using FluentValidation;

namespace LightOffSchedule.Service.Contracts;

public class UpdateLightOffScheduleByFileRequest
{
    public string FileText { get; set; } = null!;
}

public class UpdateLightOffScheduleByFileRequestValidator: AbstractValidator<UpdateLightOffScheduleByFileRequest>
{
    public UpdateLightOffScheduleByFileRequestValidator()
    {
        RuleFor(x => x.FileText).NotEmpty().WithMessage("Текст файлу порожній");
    }
}