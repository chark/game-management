using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace CHARK.GameManagement.Assets
{
    public interface IResourceLoader
    {
        /// <returns>
        /// Enumerable of resources retrieved from given <paramref name="path"/>.
        /// </returns>
        public IEnumerable<TResource> GetResources<TResource>(string path = null)
            where TResource : Object;

        /// <returns>
        /// <c>true</c> if <paramref name="resource"/> is retrieved from given
        /// <paramref name="path"/> or <c>false</c> otherwise.
        /// </returns>
        public bool TryGetResource<TResource>(string path, out TResource resource)
            where TResource : Object;

        /// <returns>
        /// Asset loaded asynchronously from given <paramref name="path"/>.
        /// <p/>
        /// <b>Note</b>, <paramref name="path"/> is relative to StreamingAssets directory.
        /// </returns>
        public Task<TResource> GetResourceAsync<TResource>(
            string path,
            CancellationToken cancellationToken = default
        );
    }
}
