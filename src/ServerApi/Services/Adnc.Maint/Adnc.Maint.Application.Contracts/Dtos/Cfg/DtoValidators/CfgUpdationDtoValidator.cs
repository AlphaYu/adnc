using Adnc.Maint.Application.Contracts.Dtos;
using FluentValidation;

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