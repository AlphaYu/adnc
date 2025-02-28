using System.Text.Json.Serialization;

namespace Adnc.Demo.Usr.Application.Contracts.Dtos.Menu;

public sealed class TDesignRouterAndPermissionsDto
{
    public List<TDesignRouterDto> Routers { get; set; } = [];
    public List<string> Permissions { get; set; } = [];
}

public sealed class TDesignRouterDto
{
    [JsonIgnore]
    public string PCode { get; set; }
    [JsonIgnore]
    public string Code { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Component { get; set; }
    public string Redirect { get; set; }
    public required RouteMeta Meta { get; set; }
    public List<TDesignRouterDto> Children { get; set; } = [];

    public class RouteMeta
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Expanded { get; set; }
        public int OrderNo { get; set; }
        public bool Hidden { get; set; }
        public bool HiddenBreadcrumb { get; set; }
        public bool Single { get; set; }
        public bool KeepAlive { get; set; }
        public string FrameSrc { get; set; }
        public bool FrameBlank { get; set; }
    }
}

