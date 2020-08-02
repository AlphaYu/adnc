using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Adnc.Application.Dtos;
using Adnc.Common.Models;
using Adnc.Application.Services;

namespace Microsoft.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// 日志记录过滤器，主要用于跟踪问题，正常情况下不需要使用
    /// </summary>
    public class LogAttribute : ActionFilterAttribute
    {
        public string LogName { get; private set; }

        public LogAttribute(string logName)
        {
            this.LogName = logName;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userContext = context.HttpContext.RequestServices.GetService<UserContext>();
            var log = new OpsLogSaveInputDto
            {
                ClassName = context.Controller.ToString(),
                CreateTime = DateTime.Now,
                LogName = this.LogName,
                LogType = "业务日志",
                Message = JsonSerializer.Serialize(context.ActionArguments),
                Method = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName,
                Succeed = "",
                UserId = userContext.ID
            };
            var logService = context.HttpContext.RequestServices.GetService<ILogAppService>();
            //await logService.Save(log);

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
