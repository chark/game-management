using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CHARK.GameManagement.Assets
{
    internal sealed class ResourceLoader : IResourceLoader
    {
        public IEnumerable<TResource> LoadResources<TResource>(string path = null) where TResource : Object
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return Resources.FindObjectsOfTypeAll<TResource>();
            }

            return Resources.LoadAll<TResource>(path);
        }

        public TResource LoadResource<TResource>(string path) where TResource : Object
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"{nameof(path)} must not be empty or null");
            }

            var resource = Resources.Load<TResource>(path);
            if (resource)
            {
                return resource;
            }

            throw new Exception($"Resource at path \"{path}\" could not be loaded");
        }
    }
}
