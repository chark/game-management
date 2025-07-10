using System.IO;
using System.Threading;
using CHARK.GameManagement.Serialization;

#if UNITASK_INSTALLED
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace CHARK.GameManagement.Storage
{
    internal abstract class Storage : IStorage
    {
        private readonly ISerializer serializer;
        private readonly string pathPrefix;

        protected Storage(ISerializer serializer, string pathPrefix)
        {
            this.serializer = serializer;
            this.pathPrefix = pathPrefix;
        }

        public virtual bool TryReadData<TValue>(string path, out TValue data)
        {
            data = default;

            if (TryGetFullPath(path, out var fullPath) == false)
            {
                return false;
            }

            using var stream = ReadStream(fullPath);
            if (stream.Length == 0)
            {
                return false;
            }

            if (serializer.TryDeserializeStream(stream, out TValue deserializedValue) == false)
            {
                return false;
            }

            data = deserializedValue;
            return true;
        }

        public Stream ReadDataStream(string path)
        {
            if (TryGetFullPath(path, out var fullPath) == false)
            {
                return Stream.Null;
            }

            return ReadStream(fullPath);
        }

#if UNITASK_INSTALLED
        public async UniTask<TData> ReadDataAsync<TData>(
#else
        public async Task<TData> ReadDataAsync<TData>(
#endif
            string path,
            CancellationToken cancellationToken = default
        )
        {
#if UNITASK_INSTALLED
            try
            {
                await UniTask.SwitchToThreadPool();
                if (TryReadData<TData>(path, out var value))
                {
                    return value;
                }
            }
            finally
            {
                await UniTask.SwitchToMainThread(cancellationToken);
            }

            return default;
#else
            return await Task.Run(() =>
                {
                    if (TryReadData<TData>(path, out var value))
                    {
                        return value;
                    }

                    return default;
                },
                cancellationToken
            );
#endif
        }

#if UNITASK_INSTALLED
        public async UniTask<Stream> ReadDataStreamAsync(
#else
        public async Task<Stream> ReadDataStreamAsync(
#endif
            string path,
            CancellationToken cancellationToken = default
        )
        {
            if (TryGetFullPath(path, out var fullPath) == false)
            {
                return Stream.Null;
            }

#if UNITASK_INSTALLED
            try
            {
                await UniTask.SwitchToThreadPool();
                return ReadStream(fullPath);
            }
            finally
            {
                await UniTask.SwitchToMainThread(cancellationToken);
            }
#else
            return await Task.Run(() => ReadStream(fullPath), cancellationToken);
#endif
        }

        public virtual void SaveData<TData>(string path, TData data)
        {
            if (TryGetFullPath(path, out var fullPath) == false)
            {
                return;
            }

            if (serializer.TrySerializeValue(data, out var serializedValue) == false)
            {
                return;
            }

            SaveString(fullPath, serializedValue);
        }

        public void SaveDataStream(string path, Stream stream)
        {
            if (TryGetFullPath(path, out var fullPath) == false)
            {
                return;
            }

            SaveStream(fullPath, stream);
        }

#if UNITASK_INSTALLED
        public async UniTask SaveDataAsync<TData>(
#else
        public async Task SaveDataAsync<TData>(
#endif
            string path,
            TData data,
            CancellationToken cancellationToken = default
        )
        {
#if UNITASK_INSTALLED
            try
            {
                await UniTask.SwitchToThreadPool();
                SaveData(path, data);
            }
            finally
            {
                await UniTask.SwitchToMainThread(cancellationToken);
            }
#else
            await Task.Run(() => SaveData(path, data), cancellationToken);
#endif
        }

#if UNITASK_INSTALLED
        public async UniTask SaveDataStreamAsync(
#else
        public async Task SaveDataStreamAsync(
#endif
            string path,
            Stream stream,
            CancellationToken cancellationToken = default
        )
        {
            if (TryGetFullPath(path, out var fullPath) == false)
            {
                return;
            }

#if UNITASK_INSTALLED
            try
            {
                await UniTask.SwitchToThreadPool();
                SaveStream(fullPath, stream);
            }
            finally
            {
                await UniTask.SwitchToMainThread(cancellationToken);
            }
#else
            await Task.Run(() => SaveStream(fullPath, stream), cancellationToken);
#endif
        }

        public void DeleteData(string path)
        {
            if (TryGetFullPath(path, out var fullPath))
            {
                Delete(fullPath);
            }
        }

#if UNITASK_INSTALLED
        public async UniTask DeleteDataAsync(
#else
        public async Task DeleteDataAsync(
#endif
            string path,
            CancellationToken cancellationToken = default
        )
        {
#if UNITASK_INSTALLED
            try
            {
                await UniTask.SwitchToThreadPool();
                Delete(path);
            }
            finally
            {
                await UniTask.SwitchToMainThread(cancellationToken);
            }
#else
            await Task.Run(() => Delete(path), cancellationToken);
#endif
        }

        /// <returns>
        /// Stream retrieved using given <paramref name="path"/>.
        /// </returns>
        protected abstract Stream ReadStream(string path);

        /// <summary>
        /// Store a <paramref name="value"/> to given <paramref name="path"/>.
        /// </summary>
        protected abstract void SaveString(string path, string value);

        /// <summary>
        /// Store a <paramref name="stream"/> to given <paramref name="path"/>.
        /// </summary>
        protected abstract void SaveStream(string path, Stream stream);

        /// <summary>
        /// Delete value at given <paramref name="path"/>.
        /// </summary>
        protected abstract void Delete(string path);

        private bool TryGetFullPath(string path, out string fullPath)
        {
            fullPath = default;

            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            fullPath = pathPrefix + path;
            return true;
        }
    }
}
