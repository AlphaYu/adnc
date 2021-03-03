using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Adnc.Application.Shared.Dtos
{
    [Serializable]
    public class PageModelDto<T> : IDto
    {

        private IReadOnlyList<T> _data = Array.Empty<T>();

        public PageModelDto() { }

        public PageModelDto(SearchPagedDto search)
        {
            this.PageIndex = search.PageIndex;
            this.PageSize = search.PageSize;
        }

        public PageModelDto(SearchPagedDto search, IReadOnlyList<T> data, int count, dynamic XData = null)
        {
            this.PageIndex = search.PageIndex;
            this.PageSize = search.PageSize;
            this.TotalCount = count;
            this.Data = data as IReadOnlyList<T>;
            this.XData = XData;
        }

        [NotNull]
        public IReadOnlyList<T> Data
        {
            get => _data;
            set
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (value != null)
                {
                    _data = value;
                }
            }
        }

        public int RowsCount { get { return _data.Count; } }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long TotalCount { get; set; }

        public int PageCount { get { return ((this.RowsCount + this.PageSize - 1) / this.PageSize); } }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public dynamic XData { get; set; }

    }
}
