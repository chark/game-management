namespace CHARK.GameManagement.Systems
{
    /// <summary>
    /// Regular system, useful to avoid having to override <see cref="ISystem"/> methods.
    /// </summary>
    public abstract class SimpleSystem : ISystem
    {
        public virtual void OnInitialized()
        {
        }

        public virtual void OnDisposed()
        {
        }
    }
}
