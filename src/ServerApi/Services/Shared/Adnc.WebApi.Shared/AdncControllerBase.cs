﻿using Microsoft.AspNetCore.Mvc;
using Adnc.Application.Shared.Services;

namespace Adnc.WebApi.Shared
{
    public abstract class AdncControllerBase : ControllerBase
    {
        /// <summary>
        /// ProblemDetails => Problem
        /// </summary>
        /// <param name="problemDetails"><see cref="Adnc.Application.Shared.Services.ProblemDetails"/></param>
        /// <returns><see cref="ObjectResult"/></returns>
        [NonAction]
        protected virtual ObjectResult Problem(Adnc.Application.Shared.Services.ProblemDetails problemDetails)
        {
            problemDetails.Instance = problemDetails.Instance ?? this.Request.Path.ToString();
            return Problem(problemDetails.Detail
                , problemDetails.Instance
                , problemDetails.Status
                , problemDetails.Title
                , problemDetails.Type);
        }

        /// <summary>
        /// AppSrvResult<TValue> => ActionResult<TValue>
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="appSrvResult"><see cref="AppSrvResult{TValue}"/></param>
        /// <returns><see cref="ActionResult{TValue}"/> if normal return status 200</returns>
        [NonAction]
        protected virtual ActionResult<TValue> Result<TValue>(AppSrvResult<TValue> appSrvResult)
        {
            if (appSrvResult.IsSuccess)
                return appSrvResult.Content;
            return Problem(appSrvResult.ProblemDetails);
        }

        /// <summary>
        /// AppSrvResult => ActionResult 
        /// </summary>
        /// <param name="appSrvResult"><see cref="AppSrvResult"/></param>
        /// <returns><see cref="ActionResult"/> if normal return statuscode 204</returns>
        [NonAction]
        protected virtual ActionResult Result(AppSrvResult appSrvResult)
        {
            if (appSrvResult.IsSuccess)
                return NoContent();
            return Problem(appSrvResult.ProblemDetails);
        }

        /// <summary>
        /// AppSrvResult<TValue> => ActionResult<TValue>
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="appSrvResult"><see cref="AppSrvResult{TValue}"/></param>
        /// <returns><see cref="ActionResult{TValue}"/> if normal return statuscode 201</returns>
        [NonAction]
        protected virtual ActionResult<TValue> CreatedResult<TValue>(AppSrvResult<TValue> appSrvResult)
        {
            if (appSrvResult.IsSuccess)
                return Created(this.Request.Path, appSrvResult.Content);
            return Problem(appSrvResult.ProblemDetails);
        }
    }
}
