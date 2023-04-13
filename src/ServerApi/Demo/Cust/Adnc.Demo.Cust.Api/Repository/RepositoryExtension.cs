using Adnc.Demo.Cust.Api.Repository.Entities;

namespace Adnc.Demo.Cust.Api.Repository
{
    public static class RepositoryExtension
    {
        /// <summary>
        ///  通过sql查询客户分页信息
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="repository"></param>
        /// <param name="condition"></param>
        /// <param name="offset"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task<QueryPageResult<TResult>> GetPagedCustmersBySqlAsync<TResult>(this IEfRepository<Customer> repository, QueryCondition condition, int offset, int rows)
            where TResult : notnull
        {
            var adoQuerier = repository.AdoQuerier ?? throw new NullReferenceException("AdoQuerier is null");
            string countSql = $"select count(*) from customer {condition.Where}";
            var total = await adoQuerier.QuerySingleAsync<int>(countSql, condition.Param);
            if (total == 0)
                return new QueryPageResult<TResult>(total);

            string sql = $"select * from customer  {condition.Where} {condition.OrderBy} limit {offset},{rows}";
            var result = await adoQuerier.QueryAsync<TResult>(sql, condition.Param);
            return new QueryPageResult<TResult>(total, result);
        }
    }
}
