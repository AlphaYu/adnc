namespace Adnc.Shared.Application.Contracts.ResultModels;

/// <summary>
/// Application-layer result wrapper without a return value (void, Task)
/// </summary>
[Serializable]
public sealed class ServiceResult
{
    public ServiceResult()
    {
    }

    public ServiceResult(ProblemDetails problemDetails) => ProblemDetails = problemDetails;

    public bool IsSuccess => ProblemDetails == null;

    public ProblemDetails ProblemDetails { get; set; } = default!;

    public static implicit operator ServiceResult(ProblemDetails problemDetails)
    {
        return new()
        {
            ProblemDetails = problemDetails
        };
    }
}

/// <summary>
/// Application result wrapper with a return value
/// </summary>
[Serializable]
public sealed class ServiceResult<TValue>
{
    public ServiceResult()
    {
    }

    public ServiceResult(TValue value) => Content = value;

    public ServiceResult(ProblemDetails problemDetails) => ProblemDetails = problemDetails;

    public bool IsSuccess => ProblemDetails == null && Content != null;

    public TValue Content { get; set; } = default!;

    public ProblemDetails ProblemDetails { get; set; } = default!;

    public static implicit operator ServiceResult<TValue>(ServiceResult result)
    {
        return new()
        {
            ProblemDetails = result.ProblemDetails
        };
    }

    public static implicit operator ServiceResult<TValue>(ProblemDetails problemDetails)
    {
        return new()
        {
            ProblemDetails = problemDetails
        };
    }

    public static implicit operator ServiceResult<TValue>(TValue value) => new(value);
}
