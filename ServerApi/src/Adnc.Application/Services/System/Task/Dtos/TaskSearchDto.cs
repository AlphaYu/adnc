using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    public class TaskSearchDto : BaseSearchDto
    {

        public long Id { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        public string Name { get; set; }

    }
}
