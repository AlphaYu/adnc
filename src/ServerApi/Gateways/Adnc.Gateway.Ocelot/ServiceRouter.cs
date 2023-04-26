namespace Adnc.Gateway.Ocelot
{
    public class ServiceRouter
    {
        public string Name
        {
            get
            {
                var name = UpstreamPathTemplate.Replace("{everything}", "").Replace("/", "-");
                if (name.Length > 1)
                    return name[1..] ;
                return name;
            }
        }

        public string UpstreamPathTemplate { get; set; } = string.Empty;

        public string Group
        {
            get
            {
                if (string.IsNullOrEmpty(UpstreamPathTemplate))
                    return string.Empty;

                var names = UpstreamPathTemplate.Replace("{everything}", "").Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (names is null)
                    return string.Empty;

                return names[0];
            }
        }

        public string Path
        {
            get => UpstreamPathTemplate.Replace("{everything}", "/index.html");
        }
    }
}
