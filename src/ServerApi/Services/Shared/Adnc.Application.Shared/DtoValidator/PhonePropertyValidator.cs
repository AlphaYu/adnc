using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace Adnc.Application.Shared.DtoValidators
{
    public class PhonePropertyValidator : PropertyValidator
    {
        public PhonePropertyValidator() : base("{PropertyName} 不是有效的手机号码")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue;
            if (value == null)
                return false;

            Match match = Regex.Match(value.ToString(), @"^((1[3,5,6,8][0-9])|(14[5,7])|(17[0,1,3,6,7,8])|(19[8,9]))\d{8}$");

            return match.Success;
        }
    }

    public static class PhoneValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> Phone<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new PhonePropertyValidator());
        }
    }
}