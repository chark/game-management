using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CHARK.GameManagement.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CHARK.GameManagement.Assets
{
    internal sealed class ResourceLoader : IResourceLoader
    {
        private readonly ISerializer serializer;

        public ResourceLoader(ISerializer serializer)
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

        public async Task<TResource> GetResourceAsync<TResource>(
            string path,
            CancellationToken cancellationToken
        )
        {
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

            return default;
        }
    }
}
