using System;
using System.Collections.Generic;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Usr.Application.Contracts.Dtos
{
    [Serializable]
    public class PageModelDto<T> : Adnc.Application.Shared.Dtos.PageModelDto<T>
    {
        public PageModelDto(SearchPagedDto search, IReadOnlyList<T> data, int count, dynamic XData = null)
        {
            this.PageIndex = search.PageIndex;
            this.PageSize = search.PageSize;
            this.TotalCount = count;
            this.Data = data as IReadOnlyList<T>;
            this.XData = XData;
        }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public dynamic XData { get; set; }
    }
}
