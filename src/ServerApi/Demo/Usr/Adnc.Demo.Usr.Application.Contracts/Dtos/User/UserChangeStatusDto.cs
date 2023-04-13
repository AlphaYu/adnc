namespace Adnc.Demo.Usr.Application.Contracts.Dtos
{
    public class UserChangeStatusDto : IDto
    {
        public long[] UserIds { get; set; }

        public int Status { get; set; }
    }
}