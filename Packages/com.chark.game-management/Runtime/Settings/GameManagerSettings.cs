using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CHARK.GameManagement.Settings
{
    internal sealed class GameManagerSettings : ScriptableObject
    {
        [Tooltip(
            "List of available settings profiles, the first active profile will be picked during " +
            "Game Manager initialization"
        )]
        [SerializeField]
        private List<GameManagerSettingsProfile> profiles = new();

        private const string SettingsAssetPath = "Assets/Resources/" + SettingsResourcePath + ".asset";
        private const string SettingsResourcePath = "GameManagerSettings";

        private static GameManagerSettings currentSettings;

        /// <summary>
        /// Currently active settings instance. Never <c>null</c>.
        /// </summary>
        internal static IGameManagerSettingsProfile ActiveProfile
        {
            get
            {
                if (currentSettings == false)
                {
                    InitializeCurrentSettings();
                }

                return GetActiveProfile(currentSettings);
            }
        }

        private static void InitializeCurrentSettings()
        {
            if (TryGetSettings(out var settings))
            {
                currentSettings = settings;
            }
            else
            {
                currentSettings = CreateSettings();
            }
        }

        private static bool TryGetSettings(out GameManagerSettings settings)
        {
            var loadedSettings = Resources.Load<GameManagerSettings>(SettingsResourcePath);
            if (loadedSettings)
            {
                settings = loadedSettings;
                return true;
            }

            settings = default;
            return false;
        }

        private static GameManagerSettings CreateSettings()
        {
            var settings = CreateInstance<GameManagerSettings>();
            settings.profiles.AddRange(GetAvailableProfiles());

#if UNITY_EDITOR
            var directoryName = Path.GetDirectoryName(SettingsAssetPath);
            if (string.IsNullOrWhiteSpace(directoryName) == false &&
                Directory.Exists(directoryName) == false)
            {
                Directory.CreateDirectory(directoryName);
            }

            UnityEditor.AssetDatabase.CreateAsset(settings, SettingsAssetPath);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            Debug.Log($"Created {nameof(GameManagerSettings)} at {SettingsAssetPath}");
            return UnityEditor.AssetDatabase
                .LoadAssetAtPath<GameManagerSettings>(SettingsAssetPath);
#else
            return settings;
#endif
        }

        private static IEnumerable<GameManagerSettingsProfile> GetAvailableProfiles()
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase
                .FindAssets($"t:{nameof(GameManagerSettingsProfile)}")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<GameManagerSettingsProfile>);
#else
            return System.Array.Empty<GameManagerSettingsProfile>();
#endif
        }

        private static IGameManagerSettingsProfile GetActiveProfile(GameManagerSettings settings)
        {
            var profiles = settings.profiles;
            if (profiles.Count == 0)
            {
                return DefaultGameManagerSettingsProfile.Instance;
            }

            foreach (var profile in profiles)
            {
                if (profile && profile.IsActiveProfile)
                {
                    return profile;
                }
            }

            var firstProfile = profiles[0];
            if (firstProfile)
            {
                firstProfile.IsActiveProfile = true;
                return firstProfile;
            }

            return DefaultGameManagerSettingsProfile.Instance;
        }
    }
}
