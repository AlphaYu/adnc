using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Adnc.Maint.Application.Dtos;
using Adnc.Infr.Common.Extensions;
using Adnc.Infr.Common.Helper;
using Adnc.Maint.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared;

namespace  Adnc.Maint.Application.Services
{
    public class TaskAppService : AppService, ITaskAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysTask> _taskRepository;
        private readonly IEfRepository<SysTaskLog> _taskLogRepository;

        public TaskAppService(IMapper mapper
            , IEfRepository<SysTask> taskRepository
            , IEfRepository<SysTaskLog> taskLogRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
            _taskLogRepository = taskLogRepository;
        }

        public async Task Delete(long Id)
        {
            await _taskRepository.DeleteAsync(new[] { Id });
        }

        public async Task<List<TaskDto>> GetList(TaskSearchDto searchDto)
        {
            List<TaskDto> result = new List<TaskDto>();

            Expression<Func<SysTask, bool>> whereCondition = x => true;
            if (!string.IsNullOrWhiteSpace(searchDto.Name))
            {
                whereCondition = whereCondition.And(x => x.Name.Contains(searchDto.Name));
            }

            var tasks = await _taskRepository.SelectAsync(t => t, whereCondition, t => t.ModifyTime, false);

            return  _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task Save(TaskSaveInputDto saveDto)
        {
            if (string.IsNullOrWhiteSpace(saveDto.Name))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.BadRequest,"请输入任务名称"));
            }
            //add
            if (saveDto.ID == 0)
            {
                var exist = await _taskRepository.ExistAsync(c => c.Name == saveDto.Name);
                if (exist)
                    throw new BusinessException(new ErrorModel(ErrorCode.BadRequest,"任务名称已经存在"));

                var enity = _mapper.Map<SysTask>(saveDto);
                //enity.ID = new Snowflake(1, 1).NextId();
                enity.ID = IdGenerater.GetNextId();

                await _taskRepository.InsertAsync(enity);
            }
            //update
            else
            {
                var exist = await _taskRepository.ExistAsync(c => c.Name == saveDto.Name && c.ID != saveDto.ID);
                if (exist)
                    throw new BusinessException(new ErrorModel(ErrorCode.BadRequest,"任务名称已经存在"));

                var enity = _mapper.Map<SysTask>(saveDto);

                await _taskRepository.UpdateAsync(enity);
            }
        }

        public async Task<PageModelDto<TaskLogDto>> GetLogPaged(TaskSearchDto searchDto)
        {
            PageModelDto<TaskLogDto> result = new PageModelDto<TaskLogDto>();

            Expression<Func<SysTaskLog, bool>> whereCondition = x => true;
            if (searchDto.Id > 0)
            {
                whereCondition = whereCondition.And(x => x.ID == searchDto.Id);
            }

            var taskLogs = await _taskLogRepository.SelectAsync(t => t, whereCondition, t => t.ID, false);

            return _mapper.Map<PageModelDto<TaskLogDto>>(taskLogs);
        }
    }
}
