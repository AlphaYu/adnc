using Adnc.Application.Shared.Dtos;
using System.Collections.Generic;

namespace Adnc.Maint.Application.Dtos
{
    public class DictCreationDto : IInputDto
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public int Ordinal { get; set; }

        public IList<DictCreationDto> Children { get; set; }
    }
}
