namespace CHARK.GameManagement.Actors
{
    internal readonly struct PhysicsUpdateContext : IUpdateContext
    {
        public float DeltaTime { get; }

        public float Time { get; }

        public PhysicsUpdateContext(float deltaTime, float time)
        {
            DeltaTime = deltaTime;
            Time = time;
        }
    }
}
