using System.Threading;
using System.Threading.Tasks;

namespace CHARK.GameManagement.Storage
{
    /// <summary>
    /// Persistent storage/config repository.
    /// </summary>
    public interface IStorage
    {
        /// <returns>
        /// Value from persistent game storage asynchronously or <c>default</c> if no value is
        /// could be retrieved.
        /// </returns>
        public Task<TValue> GetValueAsync<TValue>(
            string path,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Get a value from persistent game storage.
        /// </summary>
        public bool TryGetValue<TValue>(string path, out TValue value);

        /// <returns>
        /// Save a value to persistent game storage asynchronously.
        /// </returns>
        public Task SetValueAsync<TValue>(
            string path,
            TValue value,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Save a value to persistent game storage.
        /// </summary>
        public void SetValue<TValue>(string path, TValue value);

        /// <summary>
        /// Delete value at given <paramref name="path"/> asynchronously.
        /// </summary>
        public Task DeleteValueAsync(
            string path,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Delete value at given <paramref name="path"/>.
        /// </summary>
        public void DeleteValue(string path);
    }
}
