using System.Collections.Generic;

namespace Adnc.Core.Shared
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
}
