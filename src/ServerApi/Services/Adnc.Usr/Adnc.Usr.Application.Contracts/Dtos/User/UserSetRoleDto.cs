namespace Adnc.Usr.Application.Contracts.Dtos
{
    public class UserSetRoleDto : IInputDto
    {
        public long[] RoleIds { get; set; }
    }
}