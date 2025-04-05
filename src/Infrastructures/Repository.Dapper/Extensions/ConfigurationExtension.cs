namespace Microsoft.Extensions.Configuration;

public static class ConfigurationExtension
{
    public static (string connectionString, DbTypes dbType) GetDbConnectionInfo(this IConfiguration configuration, string dbNode)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(dbNode, nameof(dbNode));

        var dbNodeSection = configuration.GetRequiredSection(dbNode);
        var connectionString = dbNodeSection["ConnectionString"] ?? throw new InvalidDataException($"{nameof(dbNode)}:ConnectionString is null");
        var dbTypeString = dbNodeSection["DbType"] ?? throw new InvalidDataException($"{nameof(dbNode)}:DbType is null");
        var dbType = (DbTypes)Enum.Parse(typeof(DbTypes), dbTypeString.ToUpper());
        return (connectionString, dbType);
    }
}
