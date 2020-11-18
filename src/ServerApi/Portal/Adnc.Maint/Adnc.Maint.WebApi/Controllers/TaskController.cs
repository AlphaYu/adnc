using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Maint.Application.Services;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Maint.WebApi.Controllers
{
    /// <summary>
    /// 任务
    /// </summary>
    [Route("maint/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskAppService _taskAppService;

        public TaskController(ITaskAppService taskAppService)
        {
            _taskAppService = taskAppService;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="Id">ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission("taskDelete")]
        public async Task Delete([FromRoute] long Id)
        {
            await _taskAppService.Delete(Id);
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Permission("taskList")]
        public async Task<List<TaskDto>> GetList([FromQuery] TaskSearchDto searchDto)
        {
            return await _taskAppService.GetList(searchDto);
        }

        /// <summary>
        /// 获取任务执行日志
        /// </summary>
        /// <returns></returns>
        [HttpGet("logs")]
        [Permission("taskLog")]
        public async Task<PageModelDto<TaskLogDto>> GetLogPaged([FromQuery] TaskSearchDto searchDto)
        {
            return await _taskAppService.GetLogPaged(searchDto);
        }


        /// <summary>
        /// 保存任务信息
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission("taskAdd", "taskEdit")]
        public async Task Save([FromBody] TaskSaveInputDto saveDto)
        {
            await _taskAppService.Save(saveDto);
        }

    }
}