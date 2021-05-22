using System;

namespace Adnc.Infra.Common.Exceptions
{
    public class AdncArgumentNullException : ArgumentNullException, IAdncException
    {
        public AdncArgumentNullException()
            : base()
        {
        }

        public AdncArgumentNullException(string paramName)
            : base(paramName)
        {
        }

        public AdncArgumentNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AdncArgumentNullException(string paramName, string message)
            : base(paramName, message)
        {
        }
    }
}