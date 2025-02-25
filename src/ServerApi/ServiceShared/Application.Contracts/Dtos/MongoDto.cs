namespace Adnc.Shared.Application.Contracts.Dtos;

public abstract class MongoDto : IDto
{
    public string Id { get; set; } = string.Empty;
}