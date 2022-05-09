using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using Pomelo.EntityFrameworkCore.MySql.Query.Internal;

namespace Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal
{
    /// <summary>
    /// adnc sql 生成类
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<挂起>")]
    public class AdncQuerySqlGenerator : MySqlQuerySqlGenerator
    {
        protected readonly Guid ContextId;
        private bool _isQueryMaseter = false;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<挂起>")]
        public AdncQuerySqlGenerator(
            [NotNull] QuerySqlGeneratorDependencies dependencies,
            [NotNull] MySqlSqlExpressionFactory sqlExpressionFactory,
            [MaybeNull] IMySqlOptions options) : base(dependencies, options)
        {
            ContextId = Guid.NewGuid();
        }

        /// <summary>
        /// 获取IQueryable的tags
        /// </summary>
        /// <param name="selectExpression"></param>
        protected override void GenerateTagsHeaderComment(SelectExpression selectExpression)
        {
            if (selectExpression.Tags.Contains(RepositoryConsts.MYCAT_ROUTE_TO_MASTER))
            {
                _isQueryMaseter = true;
                selectExpression.Tags.Remove(RepositoryConsts.MYCAT_ROUTE_TO_MASTER);
            }
            base.GenerateTagsHeaderComment(selectExpression);
        }

        /// <summary>
        /// pomelo最终生成的sql
        /// 该方法主要是调试用
        /// </summary>
        /// <param name="selectExpression"></param>
        /// <returns></returns>
        public override IRelationalCommand GetCommand(SelectExpression selectExpression)
        {
            var command = base.GetCommand(selectExpression);
            return command;
        }

        /// <summary>
        /// 在pomelo生成查询sql前，插入mycat注解
        /// 该注解的意思是从写库读取数据
        /// </summary>
        /// <param name="selectExpression"></param>
        /// <returns></returns>
        protected override Expression VisitSelect(SelectExpression selectExpression)
        {
            /*
            /*#mycat:db_type=master*/
            /*SELECT `s`.`Password`, `s`.`Salt`, `s`.`Name`, `s`.`Email`, `s`.`RoleId`, `s`.`Account`, `s`.`ID`, `s`.`Status`
            FROM `SysUser` AS `s`
            WHERE(`s`.`IsDeleted` = FALSE) AND(`s`.`Account` = 'alpha2008')
            LIMIT 1
            */
            if (_isQueryMaseter)
                Sql.Append(string.Concat("/*", RepositoryConsts.MYCAT_ROUTE_TO_MASTER, "*/ "));

            return base.VisitSelect(selectExpression);
        }
    }
}