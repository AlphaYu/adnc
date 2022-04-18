namespace Adnc.Infra.Consul.TokenGenerator
{
    public interface ITokenGenerator
    {
        public string Scheme { get; }

        /// <summary>
        /// 创建一个token
        /// </summary>
        /// <returns></returns>
        public string Create();
    }
}