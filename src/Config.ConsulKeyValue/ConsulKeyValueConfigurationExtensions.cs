using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// TODO
    /// </summary>
    public static class ConsulKeyValueConfigurationExtensions
    {

    /// <summary>
    /// TODO
    /// </summary>
        public static IConfigurationBuilder AddConsulKeyValue(
            this IConfigurationBuilder configurationBuilder,
            Uri consulAgentUri)
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