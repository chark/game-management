namespace CHARK.GameManagement.Actors
{
    internal readonly struct FrameUpdateContext : IUpdateContext
    {
        public float DeltaTime { get; }

        public float Time { get; }

        public FrameUpdateContext(float deltaTime, float time)
        {
            DeltaTime = deltaTime;
            Time = time;
        }
    }
}
