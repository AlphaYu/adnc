namespace System.Text;

public static class StringBuilderExtension
{
    public static StringBuilder AppendIf(this StringBuilder sb, bool condition, string value)
    {
        if (value is not null && condition)
            sb.Append(value);
        return sb;
    }

    public static string ToSqlWhereString(this StringBuilder sb)
    {
        if (sb.Length > 0)
        {
            var condition = sb.ToString().Trim();
            if (condition.StartsWith("and", StringComparison.OrdinalIgnoreCase))
                condition = condition[3..];
            if (condition.StartsWith("or", StringComparison.OrdinalIgnoreCase))
                condition = condition[2..];
            return "WHERE " + condition;

        }
        return string.Empty;
    }
}