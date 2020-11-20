using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Adnc.Infr.Common.Helper
{
    public class SystemTextJsonHelper
    {
        public static JsonSerializerOptions GetAdncDefaultOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                ,
                Encoder = SystemTextJsonHelper.GetAdncDefaultEncoder()
            };
        }

        public static JavaScriptEncoder GetAdncDefaultEncoder()
        {
            return JavaScriptEncoder.Create(new TextEncoderSettings(UnicodeRanges.All));
        }
    }
}