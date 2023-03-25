namespace Adnc.Infra.Core.Configuration;

/// <summary>
/// Represents options for Kestrel server.
/// </summary>
public class KestrelOptions
{
    /// <summary>
    /// Gets or sets the endpoints for the Kestrel server.
    /// </summary>
    public IDictionary<string, Endpoint> Endpoints { get; init; } = new Dictionary<string, Endpoint>();

    /// <summary>
    /// Represents an endpoint for the Kestrel server.
    /// </summary>
    public class Endpoint
    {
        private const string DefaultProtocols = "Http1AndHttp2";

        /// <summary>
        /// Gets or sets the URL for the endpoint.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the supported protocols for the endpoint.
        /// </summary>
        public string Protocols { get; set; } = DefaultProtocols;

        /// <summary>
        /// Initializes a new instance of the Endpoint class.
        /// </summary>
        public Endpoint()
        {
        }
    }
}