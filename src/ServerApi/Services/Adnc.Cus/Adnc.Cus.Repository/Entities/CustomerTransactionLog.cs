using Adnc.Infra.Entities;

namespace Adnc.Cus.Entities
{
    /// <summary>
    /// 客户财务变动记录
    /// </summary>
    public class CustomerTransactionLog : EfBasicAuditEntity
    {
        public long CustomerId { get; set; }

        public string Account { get; set; }

        public ExchangeTypeEnum ExchangeType { get; set; }

        public ExchageStatusEnum ExchageStatus { get; set; }

        public decimal ChangingAmount { get; set; }

        public decimal Amount { get; set; }

        public decimal ChangedAmount { get; set; }

        public string Remark { get; set; }
    }

    public enum ExchangeTypeEnum
    {
        Recharge = 8000
        ,
        Order = 8008
    }

    public enum ExchageStatusEnum
    {
        Processing = 2000
        ,
        Finished = 2008
        ,
        Failed = 2016
    }
}