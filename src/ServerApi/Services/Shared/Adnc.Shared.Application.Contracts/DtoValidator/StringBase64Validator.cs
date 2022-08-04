using FluentValidation;
using FluentValidation.Validators;

namespace Adnc.Shared.Application.Contracts.DtoValidator;
public class StringBase64Validator<T> : PropertyValidator<T, string>
{
    public override string Name => nameof(StringBase64Validator<T>);

    private static readonly char[] base64CodeArray = new char[]
        {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z',
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
                'v', 'w', 'x', 'y', 'z',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/', '='
        };

    public override bool IsValid(ValidationContext<T> context, string value)
    {
        return !string.IsNullOrWhiteSpace(value) && IsBase64(value);
    }

    /// <summary>
    /// 是否base64字符串
    /// </summary>
    /// <param name="base64Str">要判断的字符串</param>
    /// <returns></returns>
    public static bool IsBase64(string base64Str)
    {
        if (string.IsNullOrEmpty(base64Str))
            return false;
        else
        {
            if (base64Str.Contains(","))
                base64Str = base64Str.Split(',')[1];
            if (base64Str.Length % 4 != 0)
                return false;
            if (base64Str.Any(c => !base64CodeArray.Contains(c)))
                return false;
        }
        try
        {
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}

public static class StringBase64ValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> Base64Str<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new StringBase64Validator<T>());
    }
}
