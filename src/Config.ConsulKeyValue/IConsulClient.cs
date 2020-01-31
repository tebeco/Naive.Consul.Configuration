using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface IConsulKeyValueClient
    {
        /// <summary>
        /// TODO
        /// </summary>
        Task<ConsulKeyValuePair[]> GetAllAsync();

        /// <summary>
        /// TODO
        /// </summary>
        Task<ConsulKeyValuePair> GetAsync(string key);

        /// <summary>
        /// TODO
        /// </summary>
        Task<ConsulKeyValuePair> GetAsync(string key, bool decodeValue);

        /// <summary>
        /// TODO
        /// </summary>
        Task<string> GetValueAsync(string key);

        /// <summary>
        /// TODO
        /// </summary>
        Task<string[]> GetKeysAsync();
    }
}