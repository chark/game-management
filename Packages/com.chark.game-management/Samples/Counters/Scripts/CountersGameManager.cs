namespace CHARK.GameManagement.Samples.Counters
{
    internal sealed class CountersGameManager : GameManager
    {
        protected override void OnInitializeEntered()
        {
            // Systems are added to the game manager before it enters system init process.
            AddActor(new CounterSystem(countThreshold: 100));
            AddActor(new StorageSystem());
        }

        protected override void OnInitializeExited()
        {
            // After systems are initialized, they can be interacted with freely in this method.
            var counterSystem = GetActor<ICounterSystem>();
            counterSystem.IsTrackCounts = true;
        }

        protected override string GetGameManagerName()
        {
            // You can set a custom name for your manager here.
            return nameof(CountersGameManager);
        }
    }
}
