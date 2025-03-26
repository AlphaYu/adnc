using Adnc.Infra.Core.Exceptions;

namespace System;

public static class ExceptionExtension
{
    public static string GetExceptionDetail(this Exception exception)
    {
        var detail = new StringBuilder();
        detail.AppendLine("***************************************");
        detail.AppendLine(string.Format(" 异常消息： {0} ", exception.Message));
        detail.AppendLine(string.Format(" 异常发生时间： {0} ", DateTime.Now));
        detail.AppendLine(string.Format(" 异常类型： {0} ", exception.HResult));
        detail.AppendLine(string.Format(" 导致当前异常的 Exception 实例： {0} ", exception.InnerException));
        detail.AppendLine(string.Format(" 导致异常的应用程序或对象的名称： {0} ", exception.Source));
        detail.AppendLine(string.Format(" 引发异常的方法： {0} ", exception.TargetSite));
        detail.AppendLine(string.Format(" 异常堆栈信息： {0} ", exception.StackTrace));
        detail.AppendLine("***************************************");

        return detail.ToString();
    }

    public static void ThrowIf(this Exception exception, Func<bool> predicate, string message)
    {
        var result = predicate.Invoke();
        if (result)
        {
            throw new BusinessException(message);
        }
    }
}