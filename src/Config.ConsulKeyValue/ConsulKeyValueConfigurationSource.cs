using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// TODO
    /// </summary>
    public class ConsulKeyValueConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// TODO
        /// </summary>
        public Uri ConsulAgentUri { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var consulHttpClient = new ConsulKeyValueHttpClient(ConsulAgentUri);

            return new ConsulKeyValueConfigurationProvider(consulHttpClient);
        }
    }
}