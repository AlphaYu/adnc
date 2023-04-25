namespace Adnc.Gateway.Ocelot
{
    public class ServiceRouter
    {
        public string ServiceName { get; set; } = string.Empty;

        public string DownstreamPathTemplate { get; set; } = string.Empty;

        public string Group
        {
            get
            {
                if (string.IsNullOrEmpty(ServiceName))
                    return string.Empty;

                var names = ServiceName.Split('-', StringSplitOptions.RemoveEmptyEntries);
                if (names is null)
                    return string.Empty;

                return names.Length > 1 ? $"{names[0]}-{names[1]}" : names[0];
            }
        }

        public string Path
        {
            get => DownstreamPathTemplate.Replace("{everything}", "/index.html");
        }
    }
}
