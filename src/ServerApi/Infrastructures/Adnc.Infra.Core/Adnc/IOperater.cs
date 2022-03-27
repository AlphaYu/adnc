namespace Adnc.Infra.Core
{
    public interface IOperater
    {
        long Id { get; set; }

        string Account { get; set; }

        string Name { get; set; }
    }
}