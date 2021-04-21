using FluentValidation;
using Adnc.Usr.Application.Contracts.Dtos;

namespace Adnc.Usr.Application.Contracts.DtoValidators
{
    public class DeptUpdationDtoValidator : AbstractValidator<DeptUpdationDto>
    {
        public DeptUpdationDtoValidator()
        {
            Include(new DeptCreationDtoValidator());
        }
    }
}