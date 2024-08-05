namespace LightOffSchedule.Common;

public class Error
{
    public Error()
    { }

    public Error(string errorMessage, string? propertyName = null)
    {
        ErrorMessage = errorMessage;
        PropertyName = propertyName;
    }
    
    public string? PropertyName { get; set; }

    public string ErrorMessage { get; set; } = null!;
}