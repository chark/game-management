using System.IO;
using System.Threading;

#if UNITASK_INSTALLED
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace CHARK.GameManagement.Storage
{
    /// <summary>
    /// Persistent storage/config repository.
    /// </summary>
    public interface IStorage
    {
        /// <returns>
        /// <c>true</c> if persisted <paramref name="data"/> of type <see cref="TData"/> is
        /// retrieved from given <paramref name="path"/> or <c>false</c> otherwise.
        /// </returns>
        public bool TryReadData<TData>(string path, out TData data);

        /// <returns>
        /// Persisted data <see cref="Stream"/> retrieved from given <paramref name="path"/>
        /// or an empty stream if no data is could be retrieved.
        /// </returns>
        public Stream ReadDataStream(string path);

        /// <returns>
        /// Persisted <see cref="TData"/> retrieved from given <paramref name="path"/>
        /// asynchronously or <c>default</c> if no data is could be retrieved.
        /// </returns>
#if UNITASK_INSTALLED
        public UniTask<TData> ReadDataAsync<TData>(
#else
        public Task<TData> ReadDataAsync<TData>(
#endif
            string path,
            CancellationToken cancellationToken = default
        );

        /// <returns>
        /// Persisted data <see cref="Stream"/> retrieved from given <paramref name="path"/>
        /// asynchronously or an empty stream if no data is could be retrieved.
        /// </returns>
#if UNITASK_INSTALLED
        public UniTask<Stream> ReadDataStreamAsync(
#else
        public Task<Stream> ReadDataStreamAsync(
#endif
            string path,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Persist <paramref name="data"/> of type <see cref="TData"/> to to given
        /// <paramref name="path"/>.
        /// </summary>
        public void SaveData<TData>(string path, TData data);

        /// <summary>
        /// Persist <paramref name="stream"/> to <paramref name="path"/>.
        /// asynchronously.
        /// </summary>
        public void SaveDataStream(string path, Stream stream);

        /// <summary>
        /// Persist <paramref name="data"/> of type <see cref="TData"/> to <paramref name="path"/>.
        /// asynchronously.
        /// </summary>
#if UNITASK_INSTALLED
        public UniTask SaveDataAsync<TData>(
#else
        public Task SaveDataAsync<TData>(
#endif
            string path,
            TData data,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Persist <paramref name="stream"/> of data to given <paramref name="path"/>.
        /// asynchronously.
        /// </summary>
#if UNITASK_INSTALLED
        public UniTask SaveDataStreamAsync(
#else
        public Task SaveDataStreamAsync(
#endif
            string path,
            Stream stream,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Delete persisted data from given <paramref name="path"/>.
        /// </summary>
        public void DeleteData(string path);

        /// <summary>
        /// Delete persisted data from given <paramref name="path"/> asynchronously.
        /// </summary>
#if UNITASK_INSTALLED
        public UniTask DeleteDataAsync(
#else
        public Task DeleteDataAsync(
#endif
            string path,
            CancellationToken cancellationToken = default
        );
    }
}
