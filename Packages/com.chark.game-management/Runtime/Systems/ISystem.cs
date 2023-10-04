namespace CHARK.GameManagement.Systems
{
    /// <summary>
    /// System which receives <see cref="GameManager"/> callbacks.
    /// </summary>
    public interface ISystem
    {
        /// <summary>
        /// Called when this system is ready to be used.
        /// </summary>
        public void OnInitialized();

        /// <summary>
        /// Called when this system is deactivated.
        /// </summary>
        public void OnDisposed();
    }
}
