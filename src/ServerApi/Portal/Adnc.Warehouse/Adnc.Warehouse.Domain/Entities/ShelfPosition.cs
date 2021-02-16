using Adnc.Core.Shared.Domain.Entities;
using Adnc.Infr.Common.Exceptions;

namespace Adnc.Warehouse.Domain.Entities
{
    public class ShelfPosition: ValueObject
    {
        private ShelfPosition()
        {

        }

        internal ShelfPosition(string code,string description)
        {
            SetCode(code);
            this.Description = description;
        }

        public string Code { get; private set; }
        public string Description { get; private set; }

        private void SetCode(string code)
        {
            this.Code = Checker.NotNullOrEmpty(code, nameof(code));
        }
    }
}
