namespace Adnc.Core.Shared.Entities.Consts
{
    public struct EntityConst
    {
        public readonly int MaxLength;

        public readonly bool IsRequired;

        private EntityConst(bool isRequired, int maxLength)
        {
            IsRequired = isRequired;
            MaxLength = maxLength;
        }

        public static implicit operator EntityConst((bool IsRequired, int MaxLength) info)
        {
            return new EntityConst(info.IsRequired, info.MaxLength);
        }
    }
}