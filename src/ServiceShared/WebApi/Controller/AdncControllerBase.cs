namespace Microsoft.AspNetCore.Mvc;

public abstract class AdncControllerBase : ControllerBase
{
    /// <summary>
    /// Adnc.Shared.Application.Services.ProblemDetails.ProblemDetails => Problem
    /// </summary>
    /// <param name="problemDetails"><see cref="Adnc.Shared.Application.Services.ProblemDetails"/></param>
    /// <returns><see cref="ObjectResult"/></returns>
    [NonAction]
    protected virtual ObjectResult Problem(Adnc.Shared.Application.Contracts.ResultModels.ProblemDetails problemDetails)
    {
        problemDetails.Instance ??= this.Request.Path.ToString();
        return Problem(problemDetails.Detail
            , problemDetails.Instance
            , problemDetails.Status
            , problemDetails.Title
            , problemDetails.Type);
    }

    /// <summary>
    ///exception => Problem
    /// </summary>
    /// <param name="problemDetails"></param>
    /// <returns></returns>
    [NonAction]
    protected virtual ObjectResult Problem(Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        var status = (int)statusCode;
        var type = string.Concat("https://httpstatuses.com/", status);
        return Problem(exception.GetExceptionDetail(), this.Request.Path.ToString(), status, exception.Message, type);
    }

    /// <summary>
    /// ServiceResult<TValue> => ActionResult<TValue>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="appSrvResult"><see cref="ServiceResult{TValue}"/></param>
    /// <returns><see cref="ActionResult{TValue}"/> if normal return status 200</returns>
    [NonAction]
    protected virtual ActionResult<TValue> Result<TValue>(ServiceResult<TValue> appSrvResult)
    {
        if (appSrvResult.IsSuccess)
            return appSrvResult.Content;
        return Problem(appSrvResult.ProblemDetails);
    }

    /// <summary>
    /// ServiceResult => ActionResult
    /// </summary>
    /// <param name="appSrvResult"><see cref="ServiceResult"/></param>
    /// <returns><see cref="ActionResult"/> if normal return statuscode 204</returns>
    [NonAction]
    protected virtual ActionResult Result(ServiceResult appSrvResult)
    {
        if (appSrvResult.IsSuccess)
            return NoContent();
        return Problem(appSrvResult.ProblemDetails);
    }

    /// <summary>
    /// ServiceResult<TValue> => ActionResult<TValue>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="appSrvResult"><see cref="ServiceResult{TValue}"/></param>
    /// <returns><see cref="ActionResult{TValue}"/> if normal return statuscode 201</returns>
    [NonAction]
    protected virtual ActionResult<TValue> CreatedResult<TValue>(ServiceResult<TValue> appSrvResult)
    {
        if (appSrvResult.IsSuccess)
            return Created(this.Request.Path, appSrvResult.Content);
        return Problem(appSrvResult.ProblemDetails);
    }
}