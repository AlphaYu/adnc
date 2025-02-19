namespace Adnc.Infra.Core.Guard;

public partial class Checker
{

    public class Argument
    {
        internal Argument()
        {
        }

        public static void NotEmpty(Guid argument, string argumentName)
        {
            if (argument == Guid.Empty)
            {
                throw new ArgumentException(string.Format("\"{0}\" can't be empty Guid.", argumentName), argumentName);
            }
        }

        public static void NotNullOrEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty((argument ?? string.Empty).Trim()))
            {
                throw new ArgumentException(string.Format("\"{0}\" can't be empty.", argumentName), argumentName);
            }
        }

        public static void NotOutOfLength(string argument, int length, string argumentName)
        {
            if (argument.Trim().Length > length)
            {
                throw new ArgumentException(string.Format("\"{0}\" no more than {1} characters.", argumentName, length), argumentName);
            }
        }

        public static void NotNull(object? argument, string argumentName, string message = "")
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName, message);
            }
        }

        public static void NotNegative(int argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotNegativeOrZero(int argument, string argumentName)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotNegative(long argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotNegativeOrZero(long argument, string argumentName)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotNegative(float argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotNegativeOrZero(float argument, string argumentName)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotNegative(decimal argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotNegativeOrZero(decimal argument, string argumentName)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotInvalidDate(DateTime argument, string argumentName)
        {
            DateTime MinDate = new DateTime(1900, 1, 1);
            DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

            if (!((argument >= MinDate) && (argument <= MaxDate)))
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotInPast(DateTime argument, string argumentName)
        {
            if (argument < DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotInFuture(DateTime argument, string argumentName)
        {
            if (argument > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotNegative(TimeSpan argument, string argumentName)
        {
            if (argument < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotNegativeOrZero(TimeSpan argument, string argumentName)
        {
            if (argument <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void NotEmpty<T>(ICollection<T> argument, string argumentName)
        {
            NotNull(argument, argumentName, "The collection can't be null");

            if (argument.Count == 0)
            {
                throw new ArgumentException("The collection can't be empty.", argumentName);
            }
        }

        public static void NotOutOfRange(int argument, int min, int max, string argumentName)
        {
            if ((argument < min) || (argument > max))
            {
                throw new ArgumentOutOfRangeException(argumentName, string.Format("{0} must be in the range \"{1}\"-\"{2}\".", argumentName, min, max));
            }
        }

        /// <summary>
        /// Equal the length
        /// </summary>
        /// <param name="sourceLength"></param>
        /// <param name="limitLength"></param>
        /// <param name="argumentName"></param>
        public static void EqualLength(int sourceLength, int limitLength, string argumentName)
        {
            if (limitLength != sourceLength)
            {
                throw new ArgumentException(argumentName, string.Format("The length of arugment {0} must be equal to {1}.", argumentName, limitLength));
            }
        }

        public static void NotExistsFile(string argument, string argumentName)
        {
            NotNullOrEmpty(argument, argumentName);

            if (!File.Exists(argument))
            {
                throw new ArgumentException(string.Format("\"{0}\" file does not exist", argumentName), argumentName);
            }
        }

        public static void NotExistsDirectory(string argument, string argumentName)
        {
            NotNullOrEmpty(argument, argumentName);

            if (!Directory.Exists(argument))
            {
                throw new ArgumentException(string.Format("\"{0}\" directory does not exist", argumentName), argumentName);
            }
        }
    }
}
