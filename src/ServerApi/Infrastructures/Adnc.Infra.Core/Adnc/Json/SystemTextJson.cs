using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Adnc.Infra.Core
{
    public static class SystemTextJson
    {
        public static JsonSerializerOptions GetAdncDefaultOptions()
             => new JsonSerializerOptions
             {
                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                ,
                 Encoder = GetAdncDefaultEncoder()
                ,
                 //该值指示是否允许、不允许或跳过注释
                 ReadCommentHandling = JsonCommentHandling.Skip
                ,
                 //dynamic与匿名类型序列化设置
                 PropertyNameCaseInsensitive = true
                ,
                 DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
             };

        public static JavaScriptEncoder GetAdncDefaultEncoder()
            => JavaScriptEncoder.Create(new TextEncoderSettings(UnicodeRanges.All));
    }
}
