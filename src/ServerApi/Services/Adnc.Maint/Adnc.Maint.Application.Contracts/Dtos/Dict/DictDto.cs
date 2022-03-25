namespace Adnc.Maint.Application.Contracts.Dtos;

[Serializable]
public class DictDto : OutputDto
{
    public string Name { get; set; }

    public int Ordinal { get; set; }

    public long? Pid { get; set; }

    public string Value { get; set; }

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