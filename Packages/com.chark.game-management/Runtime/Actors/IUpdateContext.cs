namespace CHARK.GameManagement.Actors
{
    public interface IUpdateContext
    {
        /// <summary>
        /// The number of seconds passed since the previous frame.
        /// </summary>
        public float DeltaTime { get; }

        /// <summary>
        /// Time since the begining of the game.
        /// </summary>
        public float Time { get; }
    }
}
