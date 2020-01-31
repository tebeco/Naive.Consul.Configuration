using System.Threading.Tasks;

namespace Naive.Consul.Configuration
{
    public interface IConsulKeyValueClient
    {
        Task<ConsulKeyValuePair[]> GetAllAsync();
        Task<ConsulKeyValuePair> GetAsync(string key);
        Task<ConsulKeyValuePair> GetAsync(string key, bool decodeValue);
        Task<string> GetValueAsync(string key);
        Task<string[]> GetKeysAsync();
    }
}