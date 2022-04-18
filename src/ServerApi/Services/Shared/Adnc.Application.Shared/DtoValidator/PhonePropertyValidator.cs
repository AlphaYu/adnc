using System;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace Adnc.Application.Shared.DtoValidators
{
    public class PhonePropertyValidator<T> : PropertyValidator<T, string>
    {
        public override string Name => nameof(PhonePropertyValidator<T>);

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (value.IsNullOrWhiteSpace())
                return false;

            Match match = Regex.Match(value, @"^((1[3,5,6,8][0-9])|(14[5,7])|(17[0,1,3,6,7,8])|(19[8,9]))\d{8}$");

            return match.Success;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' 不是有效的手机号码." + errorCode;
    }

    public static class PhoneValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> Phone<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.SetValidator(new PhonePropertyValidator<T>());
    }
}