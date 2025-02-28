namespace Adnc.Demo.Ord.Domain.Aggregates.OrderAggregate;

/// <summary>
/// 订单状态枚举
/// </summary>
public enum OrderStatusCodes
{
    Creating = 1000
    ,
    WaitPay = 1008
    ,
    Paying = 1016
    ,
    WaitSend = 1040
    ,
    WaitConfirm = 1048
    ,
    WaitRate = 1056
    ,
    Finished = 1064
    ,
    Canceling = 1023
    ,
    Cancelled = 1024
    ,
    Deleted = 1032
}