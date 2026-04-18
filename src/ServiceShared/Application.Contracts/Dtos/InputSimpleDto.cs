using System.ComponentModel.DataAnnotations;

namespace Adnc.Shared.Application.Contracts.Dtos;

/// <summary>
/// Used to support receiving primitive types such as string, int, and long through API form-post requests.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class InputSimpleDto<T> : IDto
    where T : notnull
{
    /// <summary>
    /// The value to pass.
    /// </summary>
    [Required]
    public T Value { get; set; } = default!;
}
