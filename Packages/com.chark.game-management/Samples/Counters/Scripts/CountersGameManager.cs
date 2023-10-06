namespace CHARK.GameManagement.Samples.Counters
{
    internal sealed class CountersGameManager : GameManager
    {
        protected override void OnBeforeInitializeSystems()
        {
            // Systems are added to the game manager before it enters system init process.
            AddSystem(new CounterSystem(countThreshold: 100));
            AddSystem(new StorageSystem());
        }

        protected override void OnAfterInitializeSystems()
        {
            // After systems are initialized, they can be interacted with freely in this method.
            var counterSystem = GetSystem<ICounterSystem>();
            counterSystem.IsTrackCounts = true;
        }

        protected override string GetGameManagerName()
        {
            // You can set a custom name for your manager here.
            return nameof(CountersGameManager);
        }
    }
}
