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

        public async Task<TValue> GetValueAsync<TValue>(
            string path,
            CancellationToken cancellationToken
        )
        {
            await UniTask.SwitchToThreadPool();

            try
            {
                if (TryGetValue<TValue>(path, out var value))
                {
                    return value;
                }
            }
            finally
            {
                await UniTask.SwitchToMainThread(cancellationToken);
            }

            return default;
        }

        public virtual bool TryGetValue<TValue>(string path, out TValue value)
        {
            value = default;

            if (TryGetFormattedPath(path, out var formattedPath) == false)
            {
                return false;
            }

            var stringValue = GetString(formattedPath);
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            if (serializer.TryDeserializeValue(stringValue, out TValue deserializedValue))
            {
                value = deserializedValue;
                return true;
            }

            return true;
        }

        public async Task SetValueAsync<TValue>(
            string path,
            TValue value,
            CancellationToken cancellationToken
        )
        {
            await UniTask.SwitchToThreadPool();

            try
            {
                SetValue(path, value);
            }
            finally
            {
                await UniTask.SwitchToMainThread(cancellationToken);
            }
        }

        public virtual void SetValue<TValue>(string path, TValue value)
        {
            if (TryGetFormattedPath(path, out var formattedPath) == false)
            {
                return;
            }

            if (serializer.TrySerializeValue(value, out var serializedValue) == false)
            {
                return;
            }

            SetString(formattedPath, serializedValue);
        }

        public async Task DeleteValueAsync(string path, CancellationToken cancellationToken)
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

        public void DeleteValue(string path)
        {
            if (TryGetFormattedPath(path, out var formattedPath))
            {
                Delete(formattedPath);
            }
        }

        /// <returns>
        /// String retrieved using given <paramref name="path"/>.
        /// </returns>
        protected abstract string GetString(string path);

        /// <summary>
        /// Store a <paramref name="value"/> at given <paramref name="path"/>.
        /// </summary>
        protected abstract void SetString(string path, string value);

        /// <summary>
        /// Delete value at given <paramref name="path"/>.
        /// </summary>
        protected abstract void Delete(string path);

        private bool TryGetFormattedPath(string path, out string formattedPath)
        {
            formattedPath = default;

            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            formattedPath = pathPrefix + path;
            return true;
        }
    }
}
