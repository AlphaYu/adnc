﻿using System;
using System.Collections.Generic;
using System.Linq;
using Adnc.Infra.Caching.Core;

namespace Adnc.Application.Shared.Caching
{
    public interface IBloomFilterFactory
    {
        IBloomFilter GetBloomFilter(string name);
    }

    public class DefaultBloomFilterFactory : IBloomFilterFactory
    {
        private readonly IEnumerable<IBloomFilter> _filters;

        public DefaultBloomFilterFactory(IEnumerable<IBloomFilter> filters)
        {
            this._filters = filters;
        }

        public IBloomFilter GetBloomFilter(string name)
        {
            ArgumentCheck.NotNullOrWhiteSpace(name, nameof(name));

            var provider = _filters.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (provider == null) throw new ArgumentException("can not find a match bloom filters!");

            return provider;
        }
    }
}
