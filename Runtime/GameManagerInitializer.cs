using CHARK.GameManagement.Settings;
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
            var profile = GameManagerSettings.ActiveProfile;
            if (profile.IsInstantiateAutomatically == false)
            {
                return;
            }

            var profileMode = profile.InstantiationMode;
            if (profileMode != targetMode)
            {
                return;
            }

            if (profile.TryGetGameManagerPrefab(out var prefab) == false)
            {
                Debug.LogError(
                    $"Game Manager Prefab is not set in {nameof(GameManagerSettings)} settings"
                );

                return;
            }

            Object.Instantiate(prefab);
        }
    }
}
