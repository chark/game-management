using System.Collections.Generic;
using UnityEngine;

namespace CHARK.GameManagement.Assets
{
    public interface IResourceLoader
    {
        /// <returns>
        /// Resources loaded from given <paramref name="path"/>.
        /// </returns>
        public IEnumerable<TResource> LoadResources<TResource>(string path = null) where TResource : Object;

        /// <returns>
        /// Resource loaded from given <paramref name="path"/>.
        /// </returns>
        public TResource LoadResource<TResource>(string path) where TResource : Object;
    }
}
