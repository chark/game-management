namespace CHARK.GameManagement.Systems
{
    /// <summary>
    /// System which receives FixedUpdate callback.
    /// </summary>
    public interface IFixedUpdateListener
    {
        /// <summary>
        /// Called during FixedUpdate loop.
        /// </summary>
        public void OnFixedUpdated(float deltaTime);
    }
}
