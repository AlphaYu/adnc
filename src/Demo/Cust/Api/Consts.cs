namespace Adnc.Demo.Cust.Api;

public static class RouteConsts
{
    public const string CustRoot = "api/cust";
}

public static class PermissionConsts
{
    public static class Customer
    {
        public const string Create = "customer-create";
        public const string Search = "customer-search";
        public const string SearchTransactionLog = "customer-search-transactionlog";
        public const string Recharge = "customer-recharge";
    }
}
