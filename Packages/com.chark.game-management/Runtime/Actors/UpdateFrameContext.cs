namespace CHARK.GameManagement.Actors
{
    internal readonly struct UpdateFrameContext : IUpdateContext
    {
        public float DeltaTime { get; }

        public float Time { get; }

        public UpdateFrameContext(float deltaTime, float time)
        {
            DeltaTime = deltaTime;
            Time = time;
        }
    }
}
