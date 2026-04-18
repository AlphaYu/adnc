using Microsoft.EntityFrameworkCore.Query;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using Pomelo.EntityFrameworkCore.MySql.Query.Internal;

namespace Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal
{
    /// <summary>
    /// ADNC SQL generator factory class
    /// </summary>

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
    public class AdncMySqlQuerySqlGeneratorFactory : MySqlQuerySqlGeneratorFactory
    {
        private readonly QuerySqlGeneratorDependencies _dependencies;
        private readonly MySqlSqlExpressionFactory _sqlExpressionFactory;
        private readonly IMySqlOptions _options;

        public AdncMySqlQuerySqlGeneratorFactory(
            [NotNull] QuerySqlGeneratorDependencies dependencies,
            ISqlExpressionFactory sqlExpressionFactory,
            IMySqlOptions options) : base(dependencies, options)
        {
            _dependencies = dependencies;
            _sqlExpressionFactory = (MySqlSqlExpressionFactory)sqlExpressionFactory;
            _options = options;
        }

        /// <summary>
        /// Overrides QuerySqlGenerator.
        /// </summary>
        /// <returns></returns>
        public override QuerySqlGenerator Create()
        {
            var result = new AdncQuerySqlGenerator(_dependencies, _sqlExpressionFactory, _options);
            return result;
        }
    }
}
