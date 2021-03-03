using System;

namespace Adnc.Application.Shared.Dtos
{
    /// <summary>
    /// 查询条件基类
    /// </summary>
    public abstract class SearchPagedDto : ISearchPagedDto
    {

        private int _pageIndex;
        private int _pageSize;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _pageIndex < 1 ? 1 : _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize < 5) _pageSize = 5;
                if (_pageSize > 100) _pageSize = 100;
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        public int SkipRows()
        {
            return (this.PageIndex - 1) * this.PageSize;
        }
    }
}
