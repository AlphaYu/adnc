using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Adnc.Infr.Common.Exceptions
{
    public class AdncException: Exception, IAdncException
    {
        public AdncException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            base.HResult = (int)statusCode;
        }
    }
}
