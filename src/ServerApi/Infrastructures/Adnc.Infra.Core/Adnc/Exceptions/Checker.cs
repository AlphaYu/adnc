﻿namespace Adnc.Infra.Core.Exceptions;

public static class Checker
{
    public static decimal GTZero(decimal value
    , [NotNull] string parameterName)
    {
        if (value <= 0)
            throw new AdncArgumentException("不能小于0", parameterName);
        return value;
    }

    public static int GTZero(int value
    , [NotNull] string parameterName)
    {
        if (value <= 0)
            throw new AdncArgumentException("不能小于0", parameterName);
        return value;
    }

    public static long GTZero(long value
        , [NotNull] string parameterName)
    {
        if (value <= 0)
            throw new AdncArgumentException("不能小于0", parameterName);
        return value;
    }

    public static T NotEmptyCollection<T>(
T value,
[NotNull] string parameterName)
    {
        if (value == null)
        {
            throw new AdncArgumentNullException(parameterName);
        }

        if (value is ICollection collection && collection.Count < 1)
        {
            throw new AdncArgumentNullException(parameterName);
        }

        return value;
    }

    public static T NotNull<T>(
        T value,
        [NotNull] string parameterName)
    {
        if (value == null)
        {
            throw new AdncArgumentNullException(parameterName);
        }

        return value;
    }

    public static T NotNull<T>(
        T value,
        [NotNull] string parameterName,
        string message)
    {
        if (value == null)
        {
            throw new AdncArgumentNullException(parameterName, message);
        }

        return value;
    }

    public static string NotNull(
        string value,
        [NotNull] string parameterName,
        int maxLength = int.MaxValue,
        int minLength = 0)
    {
        if (value == null)
        {
            throw new AdncArgumentException($"{parameterName} can not be null!", parameterName);
        }

        if (value.Length > maxLength)
        {
            throw new AdncArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
        }

        if (minLength > 0 && value.Length < minLength)
        {
            throw new AdncArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
        }

        return value;
    }

    public static string NotNullOrWhiteSpace(
        string value,
        [NotNull] string parameterName,
        int maxLength = int.MaxValue,
        int minLength = 0)
    {
        if (value.IsNullOrWhiteSpace())
        {
            throw new AdncArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);
        }

        if (value.Length > maxLength)
        {
            throw new AdncArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
        }

        if (minLength > 0 && value.Length < minLength)
        {
            throw new AdncArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
        }

        return value;
    }

    public static string NotNullOrEmpty(
        string value,
        [NotNull] string parameterName,
        int maxLength = int.MaxValue,
        int minLength = 0)
    {
        if (value.IsNullOrEmpty())
        {
            throw new AdncArgumentException($"{parameterName} can not be null or empty!", parameterName);
        }

        if (value.Length > maxLength)
        {
            throw new AdncArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
        }

        if (minLength > 0 && value.Length < minLength)
        {
            throw new AdncArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
        }

        return value;
    }

    public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, [NotNull] string parameterName)
    {
        if (value.IsNullOrEmpty())
        {
            throw new AdncArgumentException(parameterName + " can not be null or empty!", parameterName);
        }

        return value;
    }

    public static string Length(
        string value,
        [NotNull] string parameterName,
        int maxLength,
        int minLength = 0)
    {
        if (minLength > 0)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AdncArgumentException(parameterName + " can not be null or empty!", parameterName);
            }

            if (value.Length < minLength)
            {
                throw new AdncArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
            }
        }

        if (value != null && value.Length > maxLength)
        {
            throw new AdncArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
        }

        return value;
    }
}