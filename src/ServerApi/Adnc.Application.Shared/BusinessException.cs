using System;
using System.Text.Json;
using Adnc.Infr.Common.Helper;

namespace Adnc.Application.Shared
{
    public class BusinessException : Exception
    {
        public BusinessException(ErrorModel errorModel)
            : base(errorModel.ToString())
        {
            base.HResult = (int)errorModel.StatusCode;
        }
    }
}

