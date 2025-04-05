namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

[Serializable]
public class DictOptionDto
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public DictDataOption[] DictDataList { get; set; } = [];

    [Serializable]
    public class DictDataOption
    {
        public string Label { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public string TagType { get; set; } = string.Empty;
    }
}

