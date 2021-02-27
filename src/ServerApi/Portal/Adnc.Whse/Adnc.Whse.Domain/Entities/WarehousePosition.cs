using Adnc.Core.Shared.Domain.Entities;
using Adnc.Infr.Common.Exceptions;

namespace Adnc.Whse.Domain.Entities
{
    public class WarehousePosition : ValueObject
    {
        public string Code { get; }

        public string Description { get; }

        private WarehousePosition() { }

        internal WarehousePosition(string code, string description)
        {
            this.Code = Checker.NotNullOrEmpty(code, nameof(code));
            this.Description = description != null ? description.Trim() : string.Empty;
        }
    }
}
