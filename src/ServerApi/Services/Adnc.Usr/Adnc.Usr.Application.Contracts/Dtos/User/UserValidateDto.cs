using Adnc.Application.Shared.Dtos;
using System;

namespace Adnc.Usr.Application.Contracts.Dtos
{
    [Serializable]
    public class UserValidateDto : IDto
    {
        public long Id { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }

        //public string Email { get; set; }

        public string RoleIds { get; set; }

        public int Status { get; set; }

        public string ValidationVersion { get; set; }
    }
}