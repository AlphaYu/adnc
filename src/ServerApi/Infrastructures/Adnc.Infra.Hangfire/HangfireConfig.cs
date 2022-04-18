namespace Hangfire
{
    public class HangfireConfig
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 连接检查超时，单位分钟默认一分钟
        /// </summary>
        public int ConnectionCheckTimeout { get; set; } = 1;

        /// <summary>
        /// 队列轮询间隔
        /// </summary>
        public int QueuePollInterval { get; set; } = 1;

        /// <summary>
        /// 任务超时过期时间，单位分钟默认一小时
        /// </summary>
        public int JobTimeout { get; set; } = 60;

        /// <summary>
        /// 登录授权
        /// </summary>
        public Authorize[] Authorize { get; set; } = new[] { new Authorize { Login = "adnc", Password = "123456" } };
    }

    public class Authorize
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}