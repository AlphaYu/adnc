namespace Adnc.Infra.Application.Dtos
{
    public abstract class MongoDto : IDto
    {
        public string Id { get; set; }
    }
}