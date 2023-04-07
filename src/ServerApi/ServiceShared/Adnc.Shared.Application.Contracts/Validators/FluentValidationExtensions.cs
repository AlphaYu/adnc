namespace Adnc.Shared.Application.Contracts.Validators;


public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> Chinese<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches(Regulars.CHINESE)
        .WithMessage("Can only input chinese of {PropertyName}");

    public static IRuleBuilderOptions<T, string> Number<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches(Regulars.NUMBER)
        .WithMessage("Can only input number of {PropertyName}");

    public static IRuleBuilderOptions<T, string> Letter<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches(Regulars.LETTER)
        .WithMessage("Can only input letter of {PropertyName}");

    public static IRuleBuilderOptions<T, string> LowerLetter<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches(Regulars.LOWER_LETTER)
        .WithMessage("Can only input lower letter of {PropertyName}");

    public static IRuleBuilderOptions<T, string> UpperLetter<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches(Regulars.UPPER_LETTER)
        .WithMessage("Can only input upper letter of {PropertyName}");

    public static IRuleBuilderOptions<T, string> LetterNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches(Regulars.LETTER_NUMBER)
        .WithMessage("Can only input upper letter and number of {PropertyName}");

    public static IRuleBuilderOptions<T, string> LetterNumberUnderscode<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches(Regulars.LETTER_NUMBER_UNDERSCODE)
        .WithMessage("Can only input letter and number or underscode of {PropertyName}");

    public static IRuleBuilderOptions<T, string> ChineseLetter<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches(Regulars.CHINESE_LETTER)
        .WithMessage("Can only input upper chinese and letter of {PropertyName}");

    public static IRuleBuilderOptions<T, string> ChineseLetterNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches(Regulars.CHINESE_LETTER_NUMBER)
        .WithMessage("Can only input upper chinese and letter and number of {PropertyName}");

    public static IRuleBuilderOptions<T, string> Phone<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches<T>(Regulars.PHONE)
        .WithMessage("{PropertyName} format is incorrect");

    public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches(Regulars.EMAIL)
        .WithMessage("{PropertyName} format is incorrect");

    public static IRuleBuilderOptions<T, string> IdCard<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches(Regulars.IDCARD)
        .WithMessage("{PropertyName} format is incorrect");

    public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
        ruleBuilder
        .Matches(Regulars.URL)
        .WithMessage("{PropertyName} format is incorrect");

    public static IRuleBuilderOptions<T, string> MinLength<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength)
        =>
        ruleBuilder
        .MinimumLength(minimumLength)
        .WithMessage("Please enter a number greater than {MinLength} of {PropertyName}");

    public static IRuleBuilderOptions<T, string> MaxLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maximumLength)
        =>
        ruleBuilder
        .MaximumLength(maximumLength)
        .WithMessage("Please enter a number less than {MaxLength} of {PropertyName}");

    public static IRuleBuilderOptions<T, string> Port<T>(this IRuleBuilder<T, string> ruleBuilder)
        =>
         ruleBuilder
        .Matches(Regulars.PORT)
        .WithMessage("Is not a valid port {PropertyName}");

    public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        =>
        ruleBuilder
        .NotNull()
        .NotEmpty()
        .WithMessage("{PropertyName} is required");
}
