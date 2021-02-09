using FluentValidation;
using Adnc.Maint.Application.Dtos;

namespace Adnc.Maint.Application.DtoValidators
{
    public class CfgUpdationDtoValidator : AbstractValidator<CfgUpdationDto>
    {
        public CfgUpdationDtoValidator()
        {
            Include(new CfgCreationDtoValidator());
        }
    }
}
