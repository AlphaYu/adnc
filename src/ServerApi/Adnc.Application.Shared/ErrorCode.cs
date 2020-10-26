namespace Adnc.Application.Shared
{
    /// <summary>
    /// 410 Gone：所请求的资源已从这个地址转移，不再可用。
    /// 415 Unsupported Media Type：客户端要求的返回格式不支持。比如，API 只能返回 JSON 格式，但是客户端要求返回 XML 格式。
    /// 422 Unprocessable Entity ：客户端上传的附件无法处理，导致请求失败。
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 400 Bad Request：服务器不理解客户端的请求，未做任何处理
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// 401 Unauthorized：用户未提供身份验证凭据，或者没有通过身份验证。
        /// </summary>
        Unauthorized = 401,
        /// <summary>
        /// Forbidden：用户通过了身份验证，但是不具有访问资源所需的权限。
        /// </summary>
        Forbidden = 403,
        /// <summary>
        /// 404 Not Found：所请求的资源不存在，或不可用。
        /// </summary>
        NotFound = 404,
        /// <summary>
        /// 405 Method Not Allowed：用户已经通过身份验证，但是所用的 HTTP 方法不在他的权限之内。
        /// </summary>
        MethodNotAllowed = 405,
        /// <summary>
        /// 429 Too Many Requests：客户端的请求次数超过限额。
        /// </summary>
        TooManyRequests = 429,
        /// <summary>
        /// 500 Internal Server Error：客户端请求有效，服务器处理时发生了意外。
        /// </summary>
        InternalServerError = 500,
        /// <summary>
        /// 503 Service Unavailable：服务器无法处理请求，一般用于网站维护状态。
        /// </summary>
        NotImplemented = 503,
    }
}
