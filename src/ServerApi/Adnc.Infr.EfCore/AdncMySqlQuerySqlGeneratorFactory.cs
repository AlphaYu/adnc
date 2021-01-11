﻿using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Pomelo.EntityFrameworkCore.MySql.Query.Internal;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using Adnc.Infr.EfCore;

namespace Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal
{
    /// <summary>
    /// adnc sql生成工厂类
    /// </summary>
#pragma warning disable EF1001 // Internal EF Core API usage.
    public class AdncMySqlQuerySqlGeneratorFactory : MySqlQuerySqlGeneratorFactory
#pragma warning restore EF1001 // Internal EF Core API usage.
    {
        private readonly QuerySqlGeneratorDependencies _dependencies;
        private readonly MySqlSqlExpressionFactory _sqlExpressionFactory;
        private readonly IMySqlOptions _options;

        public AdncMySqlQuerySqlGeneratorFactory(
            [NotNull] QuerySqlGeneratorDependencies dependencies,
            ISqlExpressionFactory sqlExpressionFactory,
            IMySqlOptions options) : base(dependencies, sqlExpressionFactory, options)
        {
            _dependencies = dependencies;
            _sqlExpressionFactory = (MySqlSqlExpressionFactory)sqlExpressionFactory;
            _options = options;
        }

        /// <summary>
        /// 重写QuerySqlGenerator
        /// </summary>
        /// <returns></returns>
        public override QuerySqlGenerator Create()
        {
            var result = new AdncQuerySqlGenerator(_dependencies, _sqlExpressionFactory, _options);
            return result;
        }
    }

    /// <summary>
    /// adnc sql 生成类
    /// </summary>
#pragma warning disable EF1001 // Internal EF Core API usage.
    public class AdncQuerySqlGenerator : MySqlQuerySqlGenerator
#pragma warning restore EF1001 // Internal EF Core API usage.
    {
        protected readonly Guid ContextId;
        private bool _isQueryMaseter = false;

        public AdncQuerySqlGenerator(
            [NotNull] QuerySqlGeneratorDependencies dependencies,
            [NotNull] MySqlSqlExpressionFactory sqlExpressionFactory,
            [CanBeNull] IMySqlOptions options)
            : base(dependencies, sqlExpressionFactory, options)
        {
            ContextId = Guid.NewGuid();
        }

        /// <summary>
        /// 获取IQueryable的tags
        /// </summary>
        /// <param name="selectExpression"></param>
        protected override void GenerateTagsHeaderComment(SelectExpression selectExpression)
        {
            if (selectExpression.Tags.Contains(EfCoreConsts.MyCAT_ROUTE_TO_MASTER))
            {
                _isQueryMaseter = true;
                selectExpression.Tags.Remove(EfCoreConsts.MyCAT_ROUTE_TO_MASTER);
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
                Sql.Append(string.Concat("/*", EfCoreConsts.MyCAT_ROUTE_TO_MASTER, "*/ "));

            return base.VisitSelect(selectExpression);
        }
    }
}