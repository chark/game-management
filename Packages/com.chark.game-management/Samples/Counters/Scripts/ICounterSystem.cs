using CHARK.GameManagement.Actors;

namespace CHARK.GameManagement.Samples.Counters
{
    internal interface ICounterSystem : ISystem
    {
        public long FixedUpdateCount { get; }

        public long UpdateCount { get; }

        public bool IsTrackCounts { get; set; }
    }
}
