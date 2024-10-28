namespace CHARK.GameManagement.Actors
{
    internal readonly struct UpdatePhysicsContext : IUpdateContext
    {
        public float DeltaTime { get; }

        public float Time { get; }

        public UpdatePhysicsContext(float deltaTime, float time)
        {
            DeltaTime = deltaTime;
            Time = time;
        }
    }
}
