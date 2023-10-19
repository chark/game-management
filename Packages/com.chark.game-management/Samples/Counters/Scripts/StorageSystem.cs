using CHARK.GameManagement.Systems;

namespace CHARK.GameManagement.Samples.Counters
{
    internal sealed class StorageSystem : SimpleSystem, IStorageSystem
    {
        public long LoadFixedUpdateCount()
        {
            var path = GetPath(nameof(CounterSystem.FixedUpdateCount));

            // Game Manager provides an abstraction for retrieving values. It also supports json
            // serialization by default.
            if (GameManager.TryReadData<long>(path, out var data))
            {
                return data;
            }

            return 0;
        }

        public long LoadUpdateCount()
        {
            var path = GetPath(nameof(CounterSystem.UpdateCount));
            if (GameManager.TryReadData<long>(path, out var data))
            {
                return data;
            }

            return 0;
        }

        public void SaveFixedUpdateCount(long count)
        {
            var path = GetPath(nameof(CounterSystem.FixedUpdateCount));
            GameManager.SaveData(path, count);
        }

        public void SaveUpdateCount(long count)
        {
            var path = GetPath(nameof(CounterSystem.UpdateCount));
            GameManager.SaveData(path, count);
        }

        private static string GetPath(string suffix)
        {
            return $"{nameof(StorageSystem)}_${suffix}";
        }
    }
}
