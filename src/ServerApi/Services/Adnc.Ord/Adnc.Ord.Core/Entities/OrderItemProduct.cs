﻿using Adnc.Core.Shared.Entities;
using Adnc.Infra.Common.Exceptions;

namespace Adnc.Ord.Core.Entities
{
    public class OrderItemProduct : ValueObject
    {
        public long Id { get; }

        public string Name { get; }

        public decimal Price { get; }

        private OrderItemProduct()
        {
        }

        public OrderItemProduct(long id, string name, decimal price)
        {
            this.Id = Checker.GTZero(id, nameof(id));
            this.Name = Checker.NotNullOrEmpty(name, nameof(name));
            this.Price = Checker.GTZero(price, nameof(price));
        }
    }
}