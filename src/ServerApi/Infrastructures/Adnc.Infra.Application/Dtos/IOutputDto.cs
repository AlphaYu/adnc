namespace Adnc.Infra.Application.Dtos
{
    /// <summary>
    /// OutputDto基类
    /// </summary>
    public interface IOutputDto : IDto
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long Id { get; set; }
    }
}