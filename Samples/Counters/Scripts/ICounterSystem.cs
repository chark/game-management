using CHARK.GameManagement.Systems;

namespace CHARK.GameManagement.Samples.Counters
{
    internal interface ICounterSystem : ISystem
    {
        public long FixedUpdateCount { get; }

        public long UpdateCount { get; }

        public bool IsTrackCounts { get; set; }
    }
}
