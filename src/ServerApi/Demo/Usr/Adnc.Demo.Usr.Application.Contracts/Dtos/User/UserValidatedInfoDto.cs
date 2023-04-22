using Adnc.Infra.Helper;

namespace Adnc.Demo.Usr.Application.Contracts.Dtos
{
    [Serializable]
    public record UserValidatedInfoDto : IDto
    {
        public UserValidatedInfoDto(long id, string account, string name, string roleids, int status)
        {
            Id = id;
            Account = account;
            Name = name;
            RoleIds = roleids;
            Status = status;
            ValidationVersion = Guid.NewGuid().ToString("N");
        }

        public long Id { get; init; }

        public string Account { get; init; }

        public string Name { get; init; }

        //public string Email { get; set; }

        public string RoleIds { get; init; }

        public int Status { get; init; }

        public string ValidationVersion { get; init; }
    }
}