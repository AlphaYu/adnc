using Adnc.Usr.Application.Contracts.Dtos;
using FluentValidation;

namespace Adnc.Usr.Application.Contracts.DtoValidators
{
    /// <summary>
    /// DeptUpdationDtoValidator
    /// </summary>
    public class DeptUpdationDtoValidator : AbstractValidator<DeptUpdationDto>
    {
        /// <summary>
        /// DeptUpdationDtoValidator
        /// </summary>
        public DeptUpdationDtoValidator()
        {
            Include(new DeptCreationDtoValidator());
        }
    }
}