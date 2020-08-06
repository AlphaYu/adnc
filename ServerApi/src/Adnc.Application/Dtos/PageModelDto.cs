using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Common.Helper;

namespace Adnc.Application.Dtos
{
    [Serializable]
    public class PageModelDto<T> : BaseDto
    {
        public IReadOnlyList<T> Data { get; set; }

        public int Count { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long TotalCount { get; set; }

        public int PageCount { get; set; }

    }
}
