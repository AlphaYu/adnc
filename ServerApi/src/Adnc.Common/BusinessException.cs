using System;
using System.Text.Json;
using Adnc.Common.Models;

namespace Adnc.Common
{
    public class BusinessException : Exception
    {
        public BusinessException(ErrorModel errorModel)
            : base(JsonSerializer.Serialize(errorModel, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true }))
        {
            base.HResult = (int)errorModel.StatusCode;
        }
    }
}
