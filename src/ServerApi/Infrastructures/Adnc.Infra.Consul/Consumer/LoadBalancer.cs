﻿using System;
using System.Collections.Generic;

namespace Adnc.Infra.Consul.Consumer
{
    public interface ILoadBalancer
    {
        string Resolve(IList<string> services);
    }

    public class RandomLoadBalancer : ILoadBalancer
    {
        private readonly Random _random = new Random();

        public string Resolve(IList<string> services)
        {
            var index = _random.Next(services.Count);
            return services[index];
        }
    }

    public class RoundRobinLoadBalancer : ILoadBalancer
    {
        private readonly object _lock = new object();
        private int _index = 0;

        public string Resolve(IList<string> services)
        {
            lock (_lock)
            {
                if (_index >= services.Count)
                {
                    _index = 0;
                }
                return services[_index++];
            }
        }
    }

    public static class TypeLoadBalancer
    {
        public static ILoadBalancer RandomLoad = new RandomLoadBalancer();
        public static ILoadBalancer RoundRobinLoad = new RoundRobinLoadBalancer();
    }
}