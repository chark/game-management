using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CHARK.GameManagement.Serialization;
using Cysharp.Threading.Tasks;

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

        public async Task<TValue> ReadDataAsync<TValue>(
            string path,
            CancellationToken cancellationToken = default
        )
        {
            await UniTask.SwitchToThreadPool();

            try
            {
                if (TryReadData<TValue>(path, out var value))
                {
                    return value;
                }
            }
            finally
            {
                await UniTask.SwitchToMainThread(cancellationToken);
            }

            throw new Exception($"Could not read data from path: {path}");
        }

        public async Task<Stream> ReadDataStreamAsync(
            string path,
            CancellationToken cancellationToken = default
        )
        {
            if (TryGetFullPath(path, out var fullPath) == false)
            {
                return Stream.Null;
            }

            await UniTask.SwitchToThreadPool();

            try
            {
                return ReadStream(fullPath);
            }
            finally
            {
                await UniTask.SwitchToMainThread(cancellationToken);
            }
        }

        public virtual void SaveData<TValue>(string path, TValue data)
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

        public async Task SaveDataAsync<TValue>(
            string path,
            TValue data,
            CancellationToken cancellationToken = default
        )
        {
            await UniTask.SwitchToThreadPool();

            try
            {
                SaveData(path, data);
            }
            finally
            {
                await UniTask.SwitchToMainThread(cancellationToken);
            }
        }

        public async Task SaveDataStreamAsync(
            string path,
            Stream stream,
            CancellationToken cancellationToken = default
        )
        {
            if (TryGetFullPath(path, out var fullPath) == false)
            {
                return;
            }

            await UniTask.SwitchToThreadPool();

            try
            {
                SaveStream(fullPath, stream);
            }
            finally
            {
                await UniTask.SwitchToMainThread(cancellationToken);
            }
        }

        public void DeleteData(string path)
        {
            if (TryGetFullPath(path, out var fullPath))
            {
                Delete(fullPath);
            }
        }

        public async Task DeleteDataAsync(
            string path,
            CancellationToken cancellationToken = default
        )
        {
            await UniTask.SwitchToThreadPool();

            try
            {
                Delete(path);
            }
            finally
            {
                await UniTask.SwitchToMainThread(cancellationToken);
            }
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
