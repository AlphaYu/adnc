namespace Adnc.Infra.Repository.Dapper.Internal;

internal sealed class TimeStampeHandler : SqlMapper.TypeHandler<byte[]>
{
    public override void SetValue(IDbDataParameter parameter, byte[]? value)
    {
        if (value is null)
        {
            parameter.Value = null;
        }
        else
        {
            var timestampLong = BitConverter.ToInt64(value, 0);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timestampDateTime = epoch.AddMilliseconds(timestampLong);
            parameter.Value = timestampDateTime;
        }
    }

    public override byte[] Parse(object value)
    {
        switch (value)
        {
            case null:
                return [];
            case DateTime dt:
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var timestampLong = (long)(dt - epoch).TotalMilliseconds;
                return BitConverter.GetBytes(timestampLong);
            default:
                throw new System.Runtime.Serialization.SerializationException($"Dapper.TimeStampeHandler Cannot convert {value.GetType()} to {typeof(byte[])}");
        }
    }
}
