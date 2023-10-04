namespace CHARK.GameManagement.Systems
{
    /// <summary>
    /// Listener which receives Update callback.
    /// </summary>
    public interface IUpdateListener
    {
        /// <summary>
        /// Called during Update loop.
        /// </summary>
        public void OnUpdated(float deltaTime);
    }
}
