using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class DeptUpdationDtoValidator : AbstractValidator<DeptUpdationDto>
    {
        public DeptUpdationDtoValidator()
        {
            Include(new DeptCreationDtoValidator());
        }
    }
}