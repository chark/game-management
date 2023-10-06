using CHARK.GameManagement.Systems;

namespace CHARK.GameManagement.Samples.Counters
{
    internal sealed class StorageSystem : SimpleSystem, IStorageSystem
    {
        public long LoadFixedUpdateCount()
        {
            var key = GetStorageKey(nameof(CounterSystem.FixedUpdateCount));

            // Game Manager provides an abstraction for retrieving values. It also supports json
            // serialization by default.
            if (GameManager.TryGetRuntimeValue<long>(key, out var value))
            {
                return value;
            }

            return 0;
        }

        public long LoadUpdateCount()
        {
            var key = GetStorageKey(nameof(CounterSystem.UpdateCount));
            if (GameManager.TryGetRuntimeValue<long>(key, out var value))
            {
                return value;
            }

            return 0;
        }

        public void SaveFixedUpdateCount(long count)
        {
            var key = GetStorageKey(nameof(CounterSystem.FixedUpdateCount));
            GameManager.SetRuntimeValue(key, count);
        }

        public void SaveUpdateCount(long count)
        {
            var key = GetStorageKey(nameof(CounterSystem.UpdateCount));
            GameManager.SetRuntimeValue(key, count);
        }

        private static string GetStorageKey(string suffix)
        {
            return $"{nameof(StorageSystem)}_${suffix}";
        }
    }
}
