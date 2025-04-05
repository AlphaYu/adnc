namespace Adnc.Infra.Core.Exceptions;

[Serializable]
public class BusinessException(string message) : Exception(message), IAdncException
{
    public int Status { get; set; } = 521;
}
