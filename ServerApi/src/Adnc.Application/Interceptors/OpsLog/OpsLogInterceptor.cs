using System;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using Adnc.Common.Models;
using Adnc.Core.Entities;
using Adnc.Core.IRepositories;

namespace Adnc.Application.Interceptors.OpsLog
{
    public class OpsLogInterceptor : IInterceptor
    {
        private bool _isLoging = false;
        private readonly IMongoRepository<SysOperationLog> _opsLogRepository;
        private readonly UserContext _userContext;

        public OpsLogInterceptor(IMongoRepository<SysOperationLog> opsLogRepository
            ,UserContext userContext)
        {
            _opsLogRepository = opsLogRepository;
            _userContext = userContext;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            if (_isLoging)
                return;
            else
                _isLoging = true;

            var serviceMethod = invocation.Method ?? invocation.MethodInvocationTarget;

            var log = new SysOperationLog
            {
                ClassName = serviceMethod.DeclaringType.FullName,
                CreateTime = DateTime.Now,
                LogName = serviceMethod.Name,
                LogType = "操作日志",
                Message = JsonConvert.SerializeObject(invocation.Arguments),
                Method = serviceMethod.Name,
                Succeed = "",
                UserId = _userContext.ID,
                UserName = _userContext.Name,
                Account = _userContext.Account,
                RemoteIpAddress = _userContext.RemoteIpAddress
            };

            _opsLogRepository.AddAsync(log);     

        }
    }
}
