namespace CHARK.GameManagement.Settings
{
    /// <summary>
    /// Settings profile used as a fall-back in situations where no profiles can be found.
    /// </summary>
    internal sealed class DefaultGameManagerSettingsProfile : IGameManagerSettingsProfile
    {
        internal static DefaultGameManagerSettingsProfile Instance { get; } = new();

        private DefaultGameManagerSettingsProfile()
        {
        }

        public string Name => nameof(DefaultGameManagerSettingsProfile);

        public bool IsInstantiateAutomatically => true;

        public InstantiationMode InstantiationMode => InstantiationMode.BeforeSceneLoad;

        public bool IsDontDestroyOnLoad => true;

        public bool IsVerboseLogging => true;

        public bool TryGetGameManagerPrefab(out GameManager prefab)
        {
            prefab = default;
            return false;
        }
    }
}
