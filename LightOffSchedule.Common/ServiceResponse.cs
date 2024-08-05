
using FluentValidation.Results;

namespace LightOffSchedule.Common;

public class ServiceResponse
{
    public bool IsSuccess { get; set; }
    
    public IEnumerable<Error>? Errors { get; set; }
    
    public static ServiceResponse Error(ValidationResult validationResult)
    {
        return new ServiceResponse()
        {
            IsSuccess = false,
            Errors = validationResult.Errors.Select(x => new Error(x.ErrorMessage, x.PropertyName))
        };
    }

    public static ServiceResponse Success()
    {
        return new ServiceResponse()
        {
            IsSuccess = true,
        };
    }
}

public class ServiceResponse<T>: ServiceResponse
{
    public T? Result { get; set; }

    public static ServiceResponse<T> Error(params Error[] errors)
    {
        return new ServiceResponse<T>()
        {
            IsSuccess = false,
            Errors = errors
        };
    }

    public static ServiceResponse<T> Success(T result)
    {
        return new ServiceResponse<T>()
        {
            IsSuccess = true,
            Result = result
        };
    }
}