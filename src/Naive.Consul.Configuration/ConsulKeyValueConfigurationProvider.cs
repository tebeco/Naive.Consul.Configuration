using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Naive.Consul.Configuration
{
    public class ConsulKeyValueConfigurationProvider : ConfigurationProvider
    {
        private readonly IConsulKeyValueClient _consulKeyValueClient;

        public ConsulKeyValueConfigurationProvider(IConsulKeyValueClient consulKeyValueClient)
        {
            _consulKeyValueClient = consulKeyValueClient ?? throw new ArgumentNullException(nameof(consulKeyValueClient));
        }

        public override void Load() => LoadAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        private async Task LoadAllAsync()
        {
            var keyValues = await _consulKeyValueClient.GetAllAsync().ConfigureAwait(false);
            var data = new Dictionary<string, string>();

            foreach (var kv in keyValues)
            {
                var value = Encoding.UTF8.GetString(Convert.FromBase64String(kv.Value));

                data.Add(kv.Key, value);
            }

            Data = data;
        }

        //private async Task LoadAsync()
        //{
        //    var keys = await _consulKeyValueClient.GetKeysAsync().ConfigureAwait(false);
        //    var data = new Dictionary<string, string>();

        //    foreach(var key in keys)
        //    {
        //        var value = await _consulKeyValueClient.GetValueAsync(key).ConfigureAwait(false);
        //        data.Add(key, value);
        //    }

        //    Data = data;
        //}
    }
}