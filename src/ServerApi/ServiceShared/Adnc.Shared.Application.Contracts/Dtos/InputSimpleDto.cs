using System.ComponentModel.DataAnnotations;

namespace Adnc.Shared.Application.Contracts.Dtos;

/// <summary>
/// 用于解决API frompost 方式接收 string,int,long等基础类型的问题。
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class InputSimpleDto<T> : IDto
    where T : notnull
{
    /// <summary>
    /// 需要传递的值
    /// </summary>
    [Required]
    public T Value { get; set; } = default!;
}