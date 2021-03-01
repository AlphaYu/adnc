﻿using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Infr.EventBus
{
    /// <summary>
    /// 空的Cap发布者，给单元测试用
    /// </summary>
    public class NullCapPublisher : ICapPublisher
    {
        public IServiceProvider ServiceProvider { get; set; }

        public AsyncLocal<ICapTransaction> Transaction { get; set; }

        public void Publish<T>(string name, T contentObj, string callbackName = null)
        {
        }

        public void Publish<T>(string name, T contentObj, IDictionary<string, string> headers)
        {
        }

        public Task PublishAsync<T>(string name, T contentObj, string callbackName = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task PublishAsync<T>(string name, T contentObj, IDictionary<string, string> headers, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
