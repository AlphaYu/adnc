using System;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Adnc.Application.Shared
{
    public class BusinessException : Exception
    {
        public BusinessException(ErrorModel errorModel)
            : base(JsonSerializer.Serialize(errorModel, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                ,
                WriteIndented = true
                ,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }))
        {
            base.HResult = (int)errorModel.StatusCode;
        }
    }
}
