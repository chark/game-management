using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CHARK.GameManagement.Serialization;
using CHARK.GameManagement.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CHARK.GameManagement.Assets
{
    internal sealed class DefaultResourceLoader : IResourceLoader
    {
        private readonly ISerializer serializer;

        public DefaultResourceLoader(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public IEnumerable<TResource> GetResources<TResource>(string path = null) where TResource : Object
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return Resources.FindObjectsOfTypeAll<TResource>();
            }

            return Resources.LoadAll<TResource>(path);
        }

        public bool TryGetResource<TResource>(string path, out TResource resource) where TResource : Object
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                resource = default;
                return false;
            }

            var loadedResource = Resources.Load<TResource>(path);
            if (loadedResource)
            {
                resource = loadedResource;
                return true;
            }

            resource = default;
            return false;
        }

        public async Task<TResource> ReadResourceAsync<TResource>(
            string path,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"{path} cannot be null or empty");
            }

            var actualPath = Path.Combine(Application.streamingAssetsPath, path);

#if UNITY_ANDROID
            var request = UnityEngine.Networking.UnityWebRequest.Get(actualPath);
            var operation = request.SendWebRequest();

            await Cysharp.Threading.Tasks.UnityAsyncExtensions.ToUniTask(
                operation,
                cancellationToken: cancellationToken
            );

            var handler = request.downloadHandler;
            var content = handler.text;
#else
            var content = await File.ReadAllTextAsync(actualPath, cancellationToken);
#endif

            if (serializer.TryDeserializeValue<TResource>(content, out var value))
            {
                return value;
            }

            throw new Exception($"Could not retrieve resource from path: {actualPath}");
        }

#pragma warning disable CS1998
        public async Task<Stream> ReadResourceStreamAsync(
#pragma warning restore CS1998
            string path,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return Stream.Null;
            }

            var actualPath = Path.Combine(Application.streamingAssetsPath, path);

            try
            {
#if UNITY_ANDROID
                var request = UnityEngine.Networking.UnityWebRequest.Get(actualPath);
                var operation = request.SendWebRequest();

                await Cysharp.Threading.Tasks.UnityAsyncExtensions.ToUniTask(
                    operation,
                    cancellationToken: cancellationToken
                );

                var handler = request.downloadHandler;
                var data = handler.data;
                if (data == null || data.Length == 0)
                {
                    return Stream.Null;
                }

                return new MemoryStream(data);
#else
                return File.OpenRead(actualPath);
#endif
            }
            catch (Exception exception)
            {
                Logging.LogException(exception, GetType());
                return Stream.Null;
            }
        }
    }
}
