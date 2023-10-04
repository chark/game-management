using System.Threading.Tasks;

namespace CHARK.GameManagement.Storage
{
    /// <summary>
    /// Persistent storage/config repository.
    /// </summary>
    public interface IGameStorage
    {
        /// <returns>
        /// Value from persistent game storage asynchronously or <c>default</c> if no value is
        /// could be retrieved.
        /// </returns>
        public Task<TValue> GetValueAsync<TValue>(string key);

        /// <summary>
        /// Get a value from persistent game storage.
        /// </summary>
        public bool TryGetValue<TValue>(string key, out TValue value);

        /// <returns>
        /// Save a value to persistent game storage asynchronously.
        /// </returns>
        public Task SetValueAsync<TValue>(string key, TValue value);

        /// <summary>
        /// Save a value to persistent game storage.
        /// </summary>
        public void SetValue<TValue>(string key, TValue value);

        /// <summary>
        /// Delete value at given key.
        /// </summary>
        public void DeleteValue(string key);
    }
}
