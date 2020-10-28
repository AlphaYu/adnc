using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

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
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            }))
        {
            base.HResult = (int)errorModel.StatusCode;
        }
    }
}
