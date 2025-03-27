namespace Adnc.Infra.Core.Exceptions;

[Serializable]
public class InvalidVariableException : Exception, IAdncException
{
    public InvalidVariableException(string message) : base()
    {
        base.HResult = (int)HttpStatusCode.BadRequest;
    }

    public InvalidVariableException(HttpStatusCode statusCode, string message)
    : base(message)
    {
        base.HResult = (int)statusCode;
    }
}
