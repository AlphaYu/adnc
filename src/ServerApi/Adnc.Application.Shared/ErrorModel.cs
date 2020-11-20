using System.Net;
using System.Text.Json;
using Adnc.Infr.Common.Helper;

namespace Adnc.Application.Shared
{
    public class ErrorModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }

        public ErrorModel()
        {
        }

        public ErrorModel(string error)
        {
            this.Error = error;
        }

        public ErrorModel(HttpStatusCode statusCode, string error)
        {
            this.Error = error;
            this.StatusCode = statusCode;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, SystemTextJsonHelper.GetAdncDefaultOptions());
        }
    }
}
