using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Adnc.Common.Models;

namespace Adnc.Common
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
