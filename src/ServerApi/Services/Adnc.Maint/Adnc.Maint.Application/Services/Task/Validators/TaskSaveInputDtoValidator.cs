using FluentValidation;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.DtoValidators;

namespace Adnc.Maint.Application.DtoValidators
{
    public class TaskSaveInputDtoValidator : AbstractValidator<TaskSaveInputDto>
    {
        public TaskSaveInputDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
