using CHARK.GameManagement.Actors;

namespace CHARK.GameManagement.Samples.Counters
{
    internal sealed class CounterSystem :
        // You can also inherit MonoSystem to get [SerializeField] functionality.
        SimpleSystem,
        ICounterSystem
    {
        private readonly long countThreshold;
        private IStorageSystem storageSystem;

        public long FixedUpdateCount { get; private set; }

        public long UpdateCount { get; private set; }

        public bool IsTrackCounts { get; set; }

        internal CounterSystem(long countThreshold = 100)
        {
            this.countThreshold = countThreshold;
        }

        protected override void OnInitialized()
        {
            // Systems can interact with other systems as well.
            storageSystem = GameManager.GetActor<IStorageSystem>();
            RestoreState();
        }

        protected override void OnDisposed()
        {
            SaveState();
        }

        protected override void OnUpdatedPhysics(IUpdateContext context)
        {
            if (IsTrackCounts == false)
            {
                return;
            }

            FixedUpdateCount++;

            if (FixedUpdateCount % countThreshold != 0)
            {
                return;
            }

            // In this example, when update count is by a certain number, we send a message for other
            // game elements to update. This is done in intervals in order not to spam messages.
            var message = new FixedUpdateCountMessage(FixedUpdateCount);
            GameManager.Publish(message);
        }

        protected override void OnUpdatedFrame(IUpdateContext context)
        {
            if (IsTrackCounts == false)
            {
                return;
            }

            UpdateCount++;

            if (UpdateCount % countThreshold != 0)
            {
                return;
            }

            var message = new UpdateCountMessage(UpdateCount);
            GameManager.Publish(message);
        }

        private void RestoreState()
        {
            FixedUpdateCount = storageSystem.LoadFixedUpdateCount();
            UpdateCount = storageSystem.LoadUpdateCount();
        }

        private void SaveState()
        {
            storageSystem.SaveFixedUpdateCount(FixedUpdateCount);
            storageSystem.SaveUpdateCount(UpdateCount);
        }
    }
}
