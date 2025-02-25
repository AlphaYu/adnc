namespace Adnc.Infra.Core.Exceptions;

[Serializable]
public class BusinessException : Exception, IAdncException
{
    public BusinessException(string message)
        : base(message)
    {
        base.HResult = (int)HttpStatusCode.BadRequest;
    }

    public BusinessException(HttpStatusCode statusCode, string message)
    : base(message)
    {
        base.HResult = (int)statusCode;
    }
}