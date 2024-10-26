using CHARK.GameManagement.Actors;

namespace CHARK.GameManagement.Samples.Counters
{
    internal interface IStorageSystem : ISystem
    {
        public long LoadFixedUpdateCount();

        public long LoadUpdateCount();

        public void SaveFixedUpdateCount(long count);

        public void SaveUpdateCount(long count);
    }
}
