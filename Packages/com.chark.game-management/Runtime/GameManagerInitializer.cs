using CHARK.GameManagement.Settings;
using CHARK.GameManagement.Utilities;
using UnityEngine;

namespace CHARK.GameManagement
{
    internal static class GameManagerInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            OnInitialize(InstantiationMode.BeforeSceneLoad);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad()
        {
            OnInitialize(InstantiationMode.AfterSceneLoad);
        }

        private static void OnInitialize(InstantiationMode targetMode)
        {
            var settings = GameManagerSettings.Instance;
            var profile = settings.ActiveProfile;

            if (profile.IsInstantiateAutomatically == false)
            {
                return;
            }

            var profileMode = profile.InstantiationMode;
            if (profileMode != targetMode)
            {
                return;
            }

            if (profile.TryGetGameManagerPrefab(out var prefab))
            {
                CreateGameManagerFromPrefab(prefab);
                return;
            }

            Logging.LogWarning(
                $"Game Manager Prefab is not set in profile {profile.Name}"
                + $" ({profile.GetType().FullName}), using {nameof(DefaultGameManager)}",
                profile as Object ?? settings
            );

            CreateDefaultGameManager();
        }

        private static void CreateGameManagerFromPrefab(GameManager prefab)
        {
            Object.Instantiate(prefab);
        }

        private static void CreateDefaultGameManager()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<DefaultGameManager>();
        }
    }
}
