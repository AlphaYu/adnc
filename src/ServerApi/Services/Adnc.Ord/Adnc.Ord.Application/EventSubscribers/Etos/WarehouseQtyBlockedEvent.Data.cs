﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Ord.Application.EventSubscribers.Etos
{
    public class WarehouseQtyBlockedEventData
    {
        public long OrderId { get; set; }

        public bool IsSuccess { get; set; }

        public string Remark { get; set; }
    }
}