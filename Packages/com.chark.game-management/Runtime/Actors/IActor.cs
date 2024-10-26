namespace CHARK.GameManagement.Actors
{
    /// <summary>
    /// Actor represents a "facade" for a set of regular components. Actors act as APIs for the game-world. If an object
    /// needs to interact with the game world or systems in some way - it's an actor!
    /// <br/>
    /// <br/>
    /// Actors are the perfect place to interact with the <see cref="GameManager"/>. Try to avoid using it in regular
    /// components and contain all game manager interactions within actors - this will greatly improve the maintainability
    /// of your game. Though, you can still refer to the <see cref="GameManager"/> in regular components when needs be,
    /// especially when under time pressure.
    /// </summary>
    public interface IActor
    {
        /// <summary>
        /// <c>true</c> if this actor is initialized or <c>false</c> otherwise.
        /// </summary>
        public bool IsInitialized { get; }

        /// <summary>
        /// Called when this actor is ready to be used.
        /// </summary>
        public void Initialize();

        /// <summary>
        /// Called when this actor is deactivated.
        /// </summary>
        public void Dispose();

        /// <summary>
        /// Called when the fixed update step should tick.
        /// </summary>
        public void UpdatePhysics(IUpdateContext context);

        /// <summary>
        /// Called when the update step should tick.
        /// </summary>
        public void UpdateFrame(IUpdateContext context);
    }
}
