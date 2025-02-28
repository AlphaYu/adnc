namespace Adnc.Demo.Maint.Application.Dtos;

[Serializable]
public class DictDto : OutputDto
{
    public string Name { get; set; } = string.Empty;

    public int Ordinal { get; set; }

    public long? Pid { get; set; }

    private string _value;

    public string Value
    {
        get => _value is not null ? _value : string.Empty;
        set => _value = value;
    }

    private IList<DictDto> _data = Array.Empty<DictDto>();

    public IList<DictDto> Children
    {
        get => _data;
        set
        {
            if (value != null)
            {
                _data = value;
            }
        }
    }
}