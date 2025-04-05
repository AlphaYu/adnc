namespace Adnc.Infra.Repository.EfCore.MongoDB.Configuration;

public enum MongoNamingConvention
{
    /// <summary>
    /// Convert names to "lowercase" without word separators.
    /// </summary>
    LowerCase = 0,

    /// <summary>
    /// Convert names to "UPPERCASE" without word separators.
    /// </summary>
    UpperCase = 1,

    /// <summary>
    /// Convert names to "UpperCamelCase".
    /// </summary>
    Pascal = 2,

    /// <summary>
    /// Convert names to "camelCase".
    /// </summary>
    Camel = 3,

    /// <summary>
    /// Convert names to "snake_case".
    /// </summary>
    Snake = 4
}
