using Castle.DynamicProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Common.Extensions;
using Adnc.Common.Models;
using Adnc.Core.Entities;
using Adnc.Core.IRepositories;

namespace Adnc.Core.Interceptors
{
    public class UowInterceptor : IInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;

        public UowInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Intercept(IInvocation invocation)
        {
            var tran = _unitOfWork.BeginTransaction();

            try
            {
                invocation.Proceed();
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }           
        }
    }
}
