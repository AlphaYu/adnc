using System.Net;

namespace Adnc.Infr.Common.Helper
{
    public class StatusCodeChecker
    {
        public static bool Is2xx(int statusCode)
        {
            return statusCode >= 200 && statusCode <= 299;
        }

        public static bool Is2xx(HttpStatusCode statusCode)
        {
            return Is2xx((int)statusCode);
        }

        public static bool Is4xx(int statusCode)
        {
            return statusCode >= 400 && statusCode <= 499;
        }

        public static bool Is4xx(HttpStatusCode statusCode)
        {
            return Is4xx((int)statusCode);
        }

        public static bool Is5xx(int statusCode)
        {
            return statusCode >= 500 && statusCode <= 599;
        }

        public static bool Is5xx(HttpStatusCode statusCode)
        {
            return Is5xx((int)statusCode);
        }
    }
}
