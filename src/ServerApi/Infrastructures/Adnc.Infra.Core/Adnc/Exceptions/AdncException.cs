using System;
using System.Net;

namespace Adnc.Infra.Core
{
    public class BusinessException : Exception, IAdncException
    {
        public BusinessException(string message)
            : base(message)
        {
            base.HResult = (int)HttpStatusCode.Forbidden;
        }

        public BusinessException(HttpStatusCode statusCode, string message)
        : base(message)
        {
            base.HResult = (int)statusCode;
        }
    }
}