using UnityEngine;

namespace CHARK.GameManagement.Systems
{
    /// <summary>
    /// System which hooks into Unity Lifecycle. Useful to avoid having to override
    /// <see cref="ISystem"/> methods.
    /// </summary>
    public abstract class MonoSystem : MonoBehaviour, ISystem
    {
        public virtual void OnInitialized()
        {
        }

        public virtual void OnDisposed()
        {
        }
    }
}
