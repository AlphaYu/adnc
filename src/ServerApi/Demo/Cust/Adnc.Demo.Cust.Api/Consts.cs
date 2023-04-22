namespace Adnc.Demo.Cust.Api
{
    public class CachingConsts
    {
        //cache key

        //cache prefix
    }

    public class PermissionConsts
    {
        public static class Customer
        {
            public const string GetList = "customerList";
            public const string Recharge = "customerRecharge";
        }
    }

    public static class EntityConsts
    {
        /// <summary>
        /// 客户
        /// </summary>
        public static class CustomerConsts
        {
            public const int Account_MaxLength = 16;
            public const int Password_Maxlength = 32;
            public const int Nickname_MaxLength = 16;
            public const int Realname_Maxlength = 16;
        }

        /// <summary>
        /// 客户财务表
        /// </summary>
        public static class CustomerFinanceConsts
        {
            public const int Account_MaxLength = 16;
        }

        /// <summary>
        /// 客户财务变动
        /// </summary>
        public static class CustomerTransactionLogConsts
        {
            public const int Account_MaxLength = 16;
            public const int Remark_MaxLength = 64;
        }
    }

    public static class RouterPath
    {
        //public const string Root = $"{SharedConsts.ShortSolutionName}/productapi";
    }
}
