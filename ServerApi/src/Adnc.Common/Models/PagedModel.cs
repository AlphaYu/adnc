using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Adnc.Common.Helper;

namespace Adnc.Common.Models
{
    public interface IPagedModel<out T>
    {
        /// <summary>
        /// Data
        /// </summary>
        IReadOnlyList<T> Data { get; }

        int Count { get; }

        /// <summary>
        /// PageNumber
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// PageSize
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// TotalDataCount
        /// </summary>
        long TotalCount { get; set; }

        /// <summary>
        /// PageCount
        /// </summary>
        int PageCount { get; }
    }

    /// <summary>
    /// 分页Model
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    [Serializable]
    public class PagedModel<T> : IPagedModel<T>
    {
        public static readonly IPagedModel<T> Empty = new PagedModel<T>();

        private IReadOnlyList<T> _data = ArrayHelper.Empty<T>();

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

        private int _pageIndex = 1;

        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                if (value > 0)
                {
                    _pageIndex = value;
                }
            }
        }

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value > 0)
                {
                    _pageSize = value;
                }
            }
        }

        private long _totalCount;

        public long TotalCount
        {
            get => _totalCount;
            set
            {
                if (value > 0)
                {
                    _totalCount = value;
                }
            }
        }

        public int PageCount => (int)((_totalCount + _pageSize - 1) / _pageSize);

        public T this[int index] => Data[index];

        public int Count => Data.Count;
    }
}
