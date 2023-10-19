using CHARK.GameManagement.Settings;

namespace CHARK.GameManagement.Tests.Editor
{
    internal sealed class GameManagerTestProfile : IGameManagerSettingsProfile
    {
        internal static GameManagerTestProfile Instance { get; } = new();

        private GameManagerTestProfile()
        {
        }

        public string Name => "Test Profile";

        public bool IsInstantiateAutomatically => false;

        public InstantiationMode InstantiationMode => InstantiationMode.BeforeSceneLoad;

        public bool IsDontDestroyOnLoad => false;

        public bool IsVerboseLogging => true;

        public bool TryGetGameManagerPrefab(out GameManager prefab)
        {
            prefab = default;
            return false;
        }
    }
}
