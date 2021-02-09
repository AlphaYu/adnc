using FluentValidation;
using Adnc.Maint.Application.Dtos;

namespace Adnc.Maint.Application.DtoValidators
{
    public class DictUpdationDtoValidator : AbstractValidator<DictUpdationDto>
    {
        public DictUpdationDtoValidator()
        {
            Include(new DictCreationDtoValidator());
        }
    }
}
