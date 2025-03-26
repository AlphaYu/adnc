using Microsoft.Extensions.Configuration;

namespace Adnc.Infra.Repository;

public static class RepositoryExtensions
{
    public static (string connectionString, DbTypes dbType) GetDbTypeAndConnectionString(this IAdoRepository _, IConfiguration configuration, string dbTypeNode, string dbConnectionNode)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(IConfiguration));
        var dbTypeString = configuration[$"{dbTypeNode}"] ?? throw new InvalidDataException($"{nameof(dbTypeNode)} is null");
        var connectionString = configuration[$"{dbConnectionNode}"] ?? throw new InvalidDataException($"{nameof(dbConnectionNode)} is null");
        var dbType = (DbTypes)Enum.Parse(typeof(DbTypes), dbTypeString.ToUpper());
        return (connectionString, dbType);
    }
}