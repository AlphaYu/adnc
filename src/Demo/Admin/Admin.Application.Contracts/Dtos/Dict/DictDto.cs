namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Dict;

[Serializable]
public class DictDto : DictCreationDto
{
    public long Id { get; set; }
}

[Serializable]
public class DictDataDto : DictDataCreationDto
{
    public long Id { get; set; }
}
