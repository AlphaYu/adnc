namespace Adnc.Demo.Usr.Application.Contracts.Dtos.Menu;

public sealed class RouterDto
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Component { get; set; } = string.Empty;
    public string Redirect { get; set; } = string.Empty;
    public required RouteMeta Meta { get; set; }
    public List<RouterDto> Children { get; set; } = [];

    public sealed class RouteMeta
    {
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool Hidden { get; set; }
        public bool AlwaysShow { get; set; }
        public bool KeepAlive { get; set; }
    }
}

