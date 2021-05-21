namespace Adnc.Infra.Consul
{
    public interface ITokenGenerator
    {
        /// <summary>
        /// 创建一个token
        /// </summary>
        /// <returns></returns>
        public string Create();
    }
}