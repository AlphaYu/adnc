namespace System;

public static class ExceptionExtension
{
    public static string GetExceptionDetail(this Exception exception)
    {
        var detail = new StringBuilder();
        detail.Append(@"***************************************");
        detail.AppendFormat(@" 异常发生时间： {0} ", DateTime.Now);
        detail.AppendFormat(@" 异常类型： {0} ", exception.HResult);
        detail.AppendFormat(@" 导致当前异常的 Exception 实例： {0} ", exception.InnerException);
        detail.AppendFormat(@" 导致异常的应用程序或对象的名称： {0} ", exception.Source);
        detail.AppendFormat(@" 引发异常的方法： {0} ", exception.TargetSite);
        detail.AppendFormat(@" 异常堆栈信息： {0} ", exception.StackTrace);
        detail.AppendFormat(@" 异常消息： {0} ", exception.Message);
        detail.Append(@"***************************************");

        return detail.ToString();
    }
}