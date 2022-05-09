﻿namespace Adnc.Infra.Core.Configuration;

public class KestrelConfig
{
    public IDictionary<string, Endpoint> Endpoints { get; set; }

    public KestrelConfig() => Endpoints = new Dictionary<string, Endpoint>();

    public class Endpoint
    {
        public Endpoint()
        {
            if (string.IsNullOrWhiteSpace(Protocols))
                Protocols = "Http1AndHttp2";
        }

        public string Url { get; set; }

        public string Protocols { get; set; }
    }
}