using FluentValidation;
using Adnc.Usr.Application.Dtos;
using Adnc.Application.Shared.DtoValidators;

namespace Adnc.Usr.Application.DtoValidators
{
    public class UserSaveInputDtoValidator : AbstractValidator<UserSaveInputDto>
    {
        public UserSaveInputDtoValidator()
        {
            RuleFor(x => x.Account).NotEmpty().Matches(@"^[a-zA-Z][a-zA-Z0-9_]{4,15}$").WithMessage("{PropertyName} 不合法,账号必须是5～16个字符,以字母开头,可包包含字母、数字、下划线。");
            RuleFor(x => x.Name).NotEmpty().Length(2, 16);
            RuleFor(x => x.Password).NotEmpty().When(x => x.Id < 1)
                                    .Length(5, 16).When(x => x.Id < 1);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(32).EmailAddress();
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(11).Phone();
            RuleFor(x => x.Birthday).NotEmpty();
        }
    }
}
