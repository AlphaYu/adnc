namespace Adnc.Core.Shared
{
    public interface IOperater
    {
        long Id { get; set; }

        string Account { get; set; }

        string Name { get; set; }
    }

    public class Operater : IOperater
    {
        public long Id { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }
    }
}