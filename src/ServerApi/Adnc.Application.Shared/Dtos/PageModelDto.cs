using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Adnc.Application.Shared.Dtos
{
    [Serializable]
    public class PageModelDto<T> : BaseDto
    {
        private IReadOnlyList<T> _data = Array.Empty<T>();

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

        public int Count { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long TotalCount { get; set; }

        public int PageCount { get; set; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public dynamic XData { get; set; }

    }
}
