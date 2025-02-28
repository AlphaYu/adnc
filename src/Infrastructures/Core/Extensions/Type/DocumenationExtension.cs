using System.Xml;

namespace System.Reflection;

/// <summary>
/// Utility class to provide documentation for various types where available with the assembly
/// </summary>
public static class DocumenationExtension
{
    /// <summary>
    /// Provides the documentation comments for a specific type
    /// </summary>
    /// <param name="type">Type to find the documentation for</param>
    /// <returns>The XML fragment that describes the type</returns>
    /// <remarks>  Prefix in type names is T</remarks>
    public static XmlElement? GetDocumentation(this Type type) => XmlFromName(type, 'T', "");

    /// <summary>
    /// Provides the documentation comments for a specific method
    /// </summary>
    /// <param name="methodInfo">The MethodInfo (reflection data ) of the member to find documentation for</param>
    /// <returns>The XML fragment describing the method</returns>
    public static XmlElement? GetDocumentation(this MethodInfo methodInfo)
    {
        // Calculate the parameter string as this is in the member name in the XML
        var parametersString = "";
        foreach (var parameterInfo in methodInfo.GetParameters())
        {
            if (parametersString.Length > 0)
            {
                parametersString += ",";
            }
            parametersString += parameterInfo.ParameterType.FullName;
        }

        //AL: 15.04.2008 ==> BUG-FIX remove “()” if parametersString is empty

        if (methodInfo.DeclaringType is null)
        {
            return default;
        }

        if (parametersString.Length > 0)
        {
            return XmlFromName(methodInfo.DeclaringType, 'M', methodInfo.Name + "(" + parametersString + ")");
        }
        else
        {
            return XmlFromName(methodInfo.DeclaringType, 'M', methodInfo.Name);
        }
    }

    /// <summary>
    /// Provides the documentation comments for a specific member
    /// </summary>
    /// <param name="memberInfo">The MemberInfo (reflection data) or the member to find documentation for</param>
    /// <returns>The XML fragment describing the member</returns>
    public static XmlElement? GetDocumentation(this MemberInfo memberInfo)
    {
        if (memberInfo is not null)
        {
            if (memberInfo.DeclaringType is null)
                return default;

            // First character [0] of member type is prefix character in the name in the XML
            return XmlFromName(memberInfo.DeclaringType, memberInfo.MemberType.ToString()[0], memberInfo.Name);
        }
        return default;
    }

    /// <summary>
    /// Returns the Xml documenation summary comment for this member
    /// </summary>
    /// <param name="memberInfo"></param>
    /// <returns></returns>
    public static string GetSummary(this MemberInfo memberInfo)
    {
        if (memberInfo is not null)
        {
            var element = memberInfo.GetDocumentation();
            var summaryElm = element?.SelectSingleNode("summary");
            if (summaryElm == null)
            {
                return "";
            }
            return summaryElm.InnerText.Trim();
        }
        return string.Empty;
    }

    /// <summary>
    /// Gets the summary portion of a type's documenation or returns an empty string if not available
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetSummary(this Type type)
    {
        var element = type.GetDocumentation();
        var summaryElm = element?.SelectSingleNode("summary");
        if (summaryElm == null)
        {
            return string.Empty;
        }
        return summaryElm.InnerText.Trim();
    }

    /// <summary>
    /// Obtains the XML Element that describes a reflection element by searching the
    /// members for a member that has a name that describes the element.
    /// </summary>
    /// <param name="type">The type or parent type, used to fetch the assembly</param>
    /// <param name="prefix">The prefix as seen in the name attribute in the documentation XML</param>
    /// <param name="name">Where relevant, the full name qualifier for the element</param>
    /// <returns>The member that has a name that describes the specified reflection element</returns>
    private static XmlElement? XmlFromName(this Type type, char prefix, string name)
    {
        string fullName;
        if (string.IsNullOrEmpty(name))
        {
            fullName = prefix + ":" + type.FullName;
        }
        else
        {
            fullName = prefix + ":" + type.FullName + "." + name;
        }
        var xmlDoc = XmlFromAssembly(type.Assembly);
        if (xmlDoc != null)
        {
            var matchedElement = xmlDoc["doc"]?["members"]?.SelectSingleNode("member[@name='" + fullName + "']") as XmlElement;
            return matchedElement;
        }
        return default;
    }

    /// <summary>
    /// A cache used to remember Xml documentation for assemblies
    /// </summary>
    private static readonly Dictionary<Assembly, XmlDocument> Cache = new Dictionary<Assembly, XmlDocument>();

    /// <summary>
    /// A cache used to store failure exceptions for assembly lookups
    /// </summary>
    private static readonly Dictionary<Assembly, Exception> FailCache = new Dictionary<Assembly, Exception>();

    /// <summary>
    /// Obtains the documentation file for the specified assembly
    /// </summary>
    /// <param name="assembly">The assembly to find the XML document for</param>
    /// <returns>The XML document</returns>
    /// <remarks>This version uses a cache to preserve the assemblies, so that
    /// the XML file is not loaded and parsed on every single lookup</remarks>
    public static XmlDocument? XmlFromAssembly(this Assembly assembly)
    {
        if (FailCache.ContainsKey(assembly))
        {
            return default;
        }
        try
        {
            if (!Cache.ContainsKey(assembly))
            {
                // load the docuemnt into the cache
                Cache[assembly] = XmlFromAssemblyNonCached(assembly);
            }
            return Cache[assembly];
        }
        catch (Exception exception)
        {
            FailCache[assembly] = exception;
            return default;
        }
    }

    /// <summary>
    /// Loads and parses the documentation file for the specified assembly
    /// </summary>
    /// <param name="assembly">The assembly to find the XML document for</param>
    /// <returns>The XML document</returns>
    private static XmlDocument XmlFromAssemblyNonCached(Assembly assembly)
    {
        var assemblyFilename = $"file:///{assembly.Location}";
        StreamReader streamReader;
        try
        {
            streamReader = new StreamReader(Path.ChangeExtension(assemblyFilename.Substring(8), ".xml"));
        }
        catch (FileNotFoundException exception)
        {
            throw new FileNotFoundException("XML documentation not present (make sure it is turned on in project properties when building)", exception);
        }
        var xmlDocument = new XmlDocument();
        xmlDocument.Load(streamReader);
        return xmlDocument;
    }
}