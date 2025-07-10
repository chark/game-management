using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

#if UNITASK_INSTALLED
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace CHARK.GameManagement.Assets
{
    public interface IResourceLoader
    {
        /// <returns>
        /// Resources of type <see cref="TResource"/> retrieved from given <paramref name="path"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="path"/> is relative to <i>Resources</i> directory.
        /// </remarks>
        public IEnumerable<TResource> GetResources<TResource>(string path = null)
            where TResource : Object;

        /// <returns>
        /// <c>true</c> if resource of type <see cref="TResource"/> is retrieved from given
        /// <paramref name="path"/> or <c>false</c> otherwise.
        /// </returns>
        /// <remarks>
        /// <paramref name="path"/> is relative to <i>Resources</i> directory.
        /// </remarks>
        public bool TryGetResource<TResource>(string path, out TResource resource)
            where TResource : Object;

        /// <returns>
        /// Resource of type <see cref="TResource"/> retrieved from given <paramref name="path"/> or
        /// </returns>
        /// <remarks>
        /// <paramref name="path"/> is relative to <i>StreamingAssets</i> directory.
        /// </remarks>
        /// <exception cref="System.Exception">
        /// if resource could not be read.
        /// </exception>
#if UNITASK_INSTALLED
        public UniTask<TResource> ReadResourceAsync<TResource>(
#else
        public Task<TResource> ReadResourceAsync<TResource>(
#endif
            string path,
            CancellationToken cancellationToken = default
        );

        /// <returns>
        /// Resource <see cref="Stream"/> retrieved from given <paramref name="path"/>. If something
        /// fails, an empty stream is be returned.
        /// </returns>
        /// <remarks>
        /// <paramref name="path"/> is relative to <i>StreamingAssets</i> directory.
        /// </remarks>
#if UNITASK_INSTALLED
        public UniTask<Stream> ReadResourceStreamAsync(
#else
        public Task<Stream> ReadResourceStreamAsync(
#endif
            string path,
            CancellationToken cancellationToken = default
        );
    }
}
