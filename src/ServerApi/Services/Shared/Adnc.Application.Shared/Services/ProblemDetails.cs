using Adnc.Infra.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Adnc.Application.Shared.Services
{
    /// <summary>
    /// 错误信息类
    /// </summary>
    [Serializable]
    public sealed class ProblemDetails
    {
        public ProblemDetails()
        {
        }

        public ProblemDetails(HttpStatusCode? statusCode = null, string detail = null, string title = null, string instance = null, string type = null)
        {
            var status = statusCode.HasValue ? (int)statusCode.Value : (int)HttpStatusCode.BadRequest;
            Status = status;
            Title = title ?? "参数错误";
            Detail = detail;
            Instance = instance;
            Type = type ?? string.Concat("https://httpstatuses.com/", status);
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, SystemTextJsonHelper.GetAdncDefaultOptions());
        }

        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> Extensions { get; }

        [JsonPropertyName("instance")]
        public string Instance { get; set; }

        [JsonPropertyName("status")]
        public int? Status { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}