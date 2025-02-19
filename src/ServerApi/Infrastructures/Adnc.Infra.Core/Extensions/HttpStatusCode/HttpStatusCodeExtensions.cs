namespace System.Net;

public static class HttpStatusCodeExtensions
{
    public static bool Is2xx(this HttpStatusCode _, int statusCode) => statusCode >= 200 && statusCode <= 299;

    public static bool Is2xx(this HttpStatusCode statusCode) => (int)statusCode >= 200 && (int)statusCode <= 299;

    public static bool Is4xx(this HttpStatusCode _, int statusCode) => statusCode >= 400 && statusCode <= 499;

    public static bool Is4xx(this HttpStatusCode statusCode) => (int)statusCode >= 400 && (int)statusCode <= 499;

    public static bool Is5xx(this HttpStatusCode _, int statusCode) => statusCode >= 500 && statusCode <= 599;

    public static bool Is5xx(this HttpStatusCode statusCode) => (int)statusCode >= 500 && (int)statusCode <= 599;
}