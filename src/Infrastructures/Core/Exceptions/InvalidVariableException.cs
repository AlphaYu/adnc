namespace Adnc.Infra.Core.Exceptions;

[Serializable]
public class InvalidVariableException(string message) : Exception(message), IAdncException
{
    public int Status { get; set; } = 520;
}
