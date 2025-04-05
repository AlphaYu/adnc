using System.Text.RegularExpressions;

namespace Adnc.Infra.Redis.Core.Internal;

// borrowed from https://github.com/neuecc/MemcachedTranscoder/blob/master/Common/TypeHelper.cs

public static class TypeHelper
{
    /// <summary>
    /// The subtract full name regex.
    /// </summary>
#pragma warning disable SYSLIB1045 // “GeneratedRegexAttribute”。
    private static readonly Regex _subtractFullNameRegex = new(@", Version=\d+.\d+.\d+.\d+, Culture=\w+, PublicKeyToken=\w+", RegexOptions.Compiled);
#pragma warning restore SYSLIB1045 // “GeneratedRegexAttribute”。

    /// <summary>
    /// Builds the name of the type.
    /// </summary>
    /// <returns>The type name.</returns>
    /// <param name="type">Type.</param>
    public static string BuildTypeName(Type type)
    {
        if (type.AssemblyQualifiedName is null)
        {
            return string.Empty;
        }

        return _subtractFullNameRegex.Replace(type.AssemblyQualifiedName, "");
    }
}
