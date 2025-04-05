namespace System;

public static class ExceptionExtensions
{
    /// <summary>
    ///  get exception detail.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public static string GetDetail(this Exception exception)
    {
        var detail = new StringBuilder();
        detail.AppendLine("start>>>");
        detail.AppendLine(string.Format(" Message： {0} ", exception.Message));
        detail.AppendLine(string.Format(" Date： {0} ", DateTime.Now));
        detail.AppendLine(string.Format(" HResult： {0} ", exception.HResult));
        detail.AppendLine(string.Format(" Exception： {0} ", exception.InnerException));
        detail.AppendLine(string.Format(" Source： {0} ", exception.Source));
        detail.AppendLine(string.Format(" TargetSite： {0} ", exception.TargetSite));
        detail.AppendLine(string.Format(" StackTrace： {0} ", exception.StackTrace));
        detail.AppendLine("<<<end");

        return detail.ToString();
    }
}
