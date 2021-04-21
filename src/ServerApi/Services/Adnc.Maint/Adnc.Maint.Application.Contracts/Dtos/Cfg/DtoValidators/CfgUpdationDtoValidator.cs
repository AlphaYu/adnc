using FluentValidation;
using Adnc.Maint.Application.Contracts.Dtos;

namespace Adnc.Maint.Application.Contracts.DtoValidators
{
    public class CfgUpdationDtoValidator : AbstractValidator<CfgUpdationDto>
    {
        public CfgUpdationDtoValidator()
        {
            Include(new CfgCreationDtoValidator());
        }
    }
}
