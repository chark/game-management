using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

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
        public UniTask<TResource> ReadResourceAsync<TResource>(
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
        public UniTask<Stream> ReadResourceStreamAsync(
            string path,
            CancellationToken cancellationToken = default
        );
    }
}
