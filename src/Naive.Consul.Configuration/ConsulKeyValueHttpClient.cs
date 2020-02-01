using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Naive.Consul.Configuration
{
    public class ConsulKeyValueHttpClient : IConsulKeyValueClient
    {
        public const string ConsulRecurseRelativeUri = "/v1/kv/?recurse";
        public const string ConsulKeysRelativeUri = "/v1/kv/?keys";
        public const string ConsulKeyValueRelativeUriFormat = "/v1/kv/{0}";

        private readonly HttpClient _httpClient;
        private readonly Uri _consulAgentUri;

        public ConsulKeyValueHttpClient(Uri consulAgentUri)
        {
            _consulAgentUri = consulAgentUri ?? throw new ArgumentNullException(nameof(consulAgentUri));

            _httpClient = new HttpClient
            {
                BaseAddress = _consulAgentUri
            };
        }

        public async Task<ConsulKeyValuePair[]> GetAllAsync()
        {
            var httpResponseMessage = await _httpClient.GetAsync(ConsulRecurseRelativeUri).ConfigureAwait(false);

            return await DeserializeFromStreamAsync<ConsulKeyValuePair[]>(httpResponseMessage.Content).ConfigureAwait(false);
        }

        //public async Task<string> GetValueAsync(string key)
        //{
        //    var consulKeyValue = await GetAsync(key, true).ConfigureAwait(false);
        //    return consulKeyValue.Value;
        //}

        //public async Task<string[]> GetKeysAsync()
        //{
        //    var httpResponseMessage = await _httpClient.GetAsync(ConsulKeysRelativeUri).ConfigureAwait(false);
        //    //TODO : Is there a better way ?
        //    // return await DeserializeFromStringAsync(httpResponseMessage.Content).ConfigureAwait(false);
        //    return await DeserializeFromStreamAsync<string[]>(httpResponseMessage.Content).ConfigureAwait(false);
        //}

        //private async Task<string[]> DeserializeFromStringAsync(HttpContent httpContent)
        //{
        //    var keysAsString = await httpContent.ReadAsStringAsync().ConfigureAwait(false);
        //    return JsonConvert.DeserializeObject<string[]>(keysAsString);
        //}

        public async Task<ConsulKeyValuePair> GetAsync(string key)
        {
            return await GetAsync(key, true);
        }

        public async Task<ConsulKeyValuePair> GetAsync(string key, bool decodeValue)
        {
            var httpResponseMessage = await _httpClient.GetAsync(string.Format(ConsulKeyValueRelativeUriFormat, key)).ConfigureAwait(false);
            var consulKeyValueAsString = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            var consulKeyValues = JsonSerializer.Deserialize<ConsulKeyValuePair[]>(consulKeyValueAsString);

            // I have no idea if thre's a better idea than the .First() here since there should only be one key matching
            // Should i throw on multiple key match ?
            var consulKeyValue = consulKeyValues[0];

            if (decodeValue)
            {
                // Should i mutate the value or create a new one ?
                consulKeyValue.Value = Encoding.UTF8.GetString(Convert.FromBase64String(consulKeyValue.Value));
            }

            return consulKeyValue;
        }

        private async Task<T> DeserializeFromStreamAsync<T>(HttpContent httpContent)
        {
            var serializer = new JsonSerializer();
            var stream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                //TODO : this "may" be fine for gettings only keys, but not so sure
                var jArray = (JArray)serializer.Deserialize(jsonTextReader);
                return jArray.ToObject<T>();
            }
        }
    }
}