using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Adnc.Infr.Common.Extensions;

namespace Adnc.Infr.Common.Exceptions
{
    public static class Checker
    {
        public static decimal GTZero(decimal value
        , [InvokerParameterName][NotNull] string parameterName)
        {
            if (value <= 0)
                throw new AdncArgumentException("不能小于0", parameterName);
            return value;
        }

        public static int GTZero(int value
        , [InvokerParameterName][NotNull] string parameterName)
        {
            if (value <= 0)
                throw new AdncArgumentException("不能小于0", parameterName);
            return value;
        }

        public static long GTZero(long value
            , [InvokerParameterName][NotNull] string parameterName)
        {
            if (value <= 0)
                throw new AdncArgumentException("不能小于0", parameterName);
            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            T value, 
            [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value == null)
            {
                throw new AdncArgumentNullException(parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            T value, 
            [InvokerParameterName] [NotNull] string parameterName, 
            string message)
        {
            if (value == null)
            {
                throw new AdncArgumentNullException(parameterName, message);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string NotNull(
            string value,
            [InvokerParameterName] [NotNull] string parameterName,
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

        [ContractAnnotation("value:null => halt")]
        public static string NotNullOrWhiteSpace(
            string value,
            [InvokerParameterName] [NotNull] string parameterName,
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

        [ContractAnnotation("value:null => halt")]
        public static string NotNullOrEmpty(
            string value,
            [InvokerParameterName] [NotNull] string parameterName,
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

        [ContractAnnotation("value:null => halt")]
        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value.IsNullOrEmpty())
            {
                throw new AdncArgumentException(parameterName + " can not be null or empty!", parameterName);
            }

            return value;
        }

        public static string Length(
            [CanBeNull] string value,
            [InvokerParameterName] [NotNull] string parameterName, 
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
}