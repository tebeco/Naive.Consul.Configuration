using System;
using Naive.Consul.Configuration;

namespace Microsoft.Extensions.Configuration
{
    public static class ConsulKeyValueConfigurationExtensions
    {
        public static IConfigurationBuilder AddConsulKeyValue(this IConfigurationBuilder configurationBuilder, Uri consulAgentUri)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }
            if (consulAgentUri == null)
            {
                throw new ArgumentNullException(nameof(consulAgentUri));
            }

            configurationBuilder.Add(new ConsulKeyValueConfigurationSource()
            {
                ConsulAgentUri = consulAgentUri
            });

            return configurationBuilder;
        }
    }
}