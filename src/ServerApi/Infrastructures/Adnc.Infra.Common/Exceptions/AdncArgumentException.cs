using System;

namespace Adnc.Infra.Common.Exceptions
{
    public class AdncArgumentException : ArgumentException, IAdncException
    {
        public AdncArgumentException()
            : base()
        {
        }

        public AdncArgumentException(string message)
            : base(message)
        {
        }

        public AdncArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AdncArgumentException(string message, string paramName)
            : base(message, paramName)
        {
        }

        public AdncArgumentException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException)
        {
        }
    }
}