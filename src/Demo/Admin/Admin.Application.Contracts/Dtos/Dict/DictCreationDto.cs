namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

public class DictCreationDto : InputDto
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Remark { get; set; } = string.Empty;

    public bool Status { get; set; }
}

public class DictDataCreationDto : InputDto
{
    public string DictCode { get; set; } = string.Empty;

    public string Label { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string TagType { get; set; } = string.Empty;

    public bool Status { get; set; }

    public int Ordinal { get; set; }
}

