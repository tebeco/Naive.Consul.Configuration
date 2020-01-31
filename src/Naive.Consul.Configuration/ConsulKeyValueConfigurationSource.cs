using System;
using Microsoft.Extensions.Configuration;

namespace Naive.Consul.Configuration
{
    public class ConsulKeyValueConfigurationSource : IConfigurationSource
    {
        public Uri ConsulAgentUri { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var consulHttpClient = new ConsulKeyValueHttpClient(ConsulAgentUri);

            return new ConsulKeyValueConfigurationProvider(consulHttpClient);
        }
    }
}