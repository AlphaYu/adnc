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
            : this(search, default, default)
        {
        }

        public PageModelDto(SearchPagedDto search, IReadOnlyList<T> data, int count, dynamic xData = null)
            : this(search.PageIndex, search.PageSize, data, count)
        {
            this.XData = xData;
        }

        public PageModelDto(int pageIndex, int pageSize, IReadOnlyList<T> data, int count, dynamic xData = null)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = count;
            this.Data = data;
            this.XData = xData;
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

        public int TotalCount { get; set; }

        public int PageCount { get { return ((this.RowsCount + this.PageSize - 1) / this.PageSize); } }

        public dynamic XData { get; set; }
    }

    //[Serializable]
    //public class PageModelDto<T, TXData> : PageModelDto<T>
    //{
    //    public PageModelDto(SearchPagedDto search, IReadOnlyList<T> data, int count, dynamic xData = null)
    //        : this(search.PageIndex, search.PageSize, data, count)
    //    {
    //    }

    //    public PageModelDto(int pageIndex, int pageSize, IReadOnlyList<T> data, int count, dynamic xData = null)
    //        : base(pageIndex, pageSize, data, count)
    //    {
    //        this.XData = xData;
    //    }

    //    public TXData XData { get; set; }

    //    public static PageModelDto<T, TXData> Convert(PageModelDto<T> source, TXData xData)
    //    {
    //        return new PageModelDto<T, TXData>(source.PageIndex, source.PageSize, source.Data, source.TotalCount, xData);
    //    }
    //}
}
