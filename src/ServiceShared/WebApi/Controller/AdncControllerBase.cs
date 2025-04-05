namespace Microsoft.AspNetCore.Mvc;

public abstract class AdncControllerBase : ControllerBase
{
    /// <summary>
    /// Adnc.Shared.Application.Services.ProblemDetails.ProblemDetails => Problem
    /// </summary>
    /// <param name="problemDetails"></param>
    /// <returns></returns>
    [NonAction]
    protected virtual ObjectResult Problem(Adnc.Shared.Application.Contracts.ResultModels.ProblemDetails problemDetails)
    {
        problemDetails.Instance ??= Request.Path.ToString();
        return Problem(problemDetails.Detail
            , problemDetails.Instance
            , problemDetails.Status
            , problemDetails.Title
            , problemDetails.Type);
    }

    /// <summary>
    /// exception => Problem
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    [NonAction]
    protected virtual ObjectResult Problem(Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        var status = (int)statusCode;
        var type = string.Concat("https://httpstatuses.com/", status);
        return Problem(exception.GetDetail(), Request.Path.ToString(), status, exception.Message, type);
    }

    /// <summary>
    /// ServiceResult{TValue} => ActionResult{TValue}
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="appSrvResult"><see cref="ServiceResult{TValue}"/></param>
    /// <returns><see cref="ActionResult{TValue}"/> if normal return status 200</returns>
    [NonAction]
    protected virtual ActionResult<TValue> Result<TValue>(ServiceResult<TValue> appSrvResult)
    {
        if (appSrvResult.IsSuccess)
        {
            return appSrvResult.Content;
        }

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
        {
            return NoContent();
        }

        return Problem(appSrvResult.ProblemDetails);
    }

    /// <summary>
    /// ServiceResult {TValue} => ActionResult{TValue}
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="appSrvResult"><see cref="ServiceResult{TValue}"/></param>
    /// <returns><see cref="ActionResult{TValue}"/> if normal return statuscode 201</returns>
    [NonAction]
    protected virtual ActionResult<TValue> CreatedResult<TValue>(ServiceResult<TValue> appSrvResult)
    {
        if (appSrvResult.IsSuccess)
        {
            return Created(Request.Path, appSrvResult.Content);
        }

        return Problem(appSrvResult.ProblemDetails);
    }
}
