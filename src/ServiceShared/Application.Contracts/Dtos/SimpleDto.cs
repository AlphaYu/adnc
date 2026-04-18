namespace Adnc.Shared.Application.Contracts.Dtos;

/// <summary>
/// Used to wrap primitive return types
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class SimpleDto<T> : IDto
    where T : notnull
{
    public SimpleDto()
    {
    }

    public SimpleDto(T value)
    {
        Value = value;
    }

    /// <summary>
    /// The value to pass.
    /// </summary>
    public T Value { get; set; } = default!;
}
