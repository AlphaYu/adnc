using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Adnc.Infr.Common.Extensions;

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
                ,
                //该值指示是否允许、不允许或跳过注释
                ReadCommentHandling = JsonCommentHandling.Skip
                ,
                //dynamic与匿名类型序列化设置
                PropertyNameCaseInsensitive = true
                ,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public static JavaScriptEncoder GetAdncDefaultEncoder()
        {
            return JavaScriptEncoder.Create(new TextEncoderSettings(UnicodeRanges.All));
        }
    }
}