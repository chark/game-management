using System.IO;
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
        /// <exception cref="System.Exception">
        /// if <see cref="TData"/> could not be retrieved.
        /// </exception>
        public Task<TData> ReadDataAsync<TData>(
            string path,
            CancellationToken cancellationToken = default
        );

        /// <returns>
        /// Persisted data <see cref="Stream"/> retrieved from given <paramref name="path"/>
        /// asynchronously or an empty stream if no data is could be retrieved.
        /// </returns>
        public Task<Stream> ReadDataStreamAsync(
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
        public Task SaveDataAsync<TData>(
            string path,
            TData data,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Persist <paramref name="stream"/> of data to given <paramref name="path"/>.
        /// asynchronously.
        /// </summary>
        public Task SaveDataStreamAsync(
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
        public Task DeleteDataAsync(
            string path,
            CancellationToken cancellationToken = default
        );
    }
}
