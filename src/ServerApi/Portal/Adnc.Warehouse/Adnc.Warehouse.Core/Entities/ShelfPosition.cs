using Adnc.Core.Shared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Warehouse.Core.Entities
{
    public class ShelfPosition: ValueObject
    {
        private ShelfPosition()
        {

        }

        internal ShelfPosition(string code,string description)
        {
            this.Code = code;
            this.Description = description;
        }

        public string Code { get; private set; }
        public string Description { get; private set; }
    }
}
