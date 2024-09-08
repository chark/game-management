using System.Collections.Generic;
using System.Linq;
using CHARK.GameManagement.Utilities;
using UnityEngine;

namespace CHARK.GameManagement.Settings
{
    internal sealed class GameManagerSettings : ScriptableObject
    {
        [Tooltip(
            "List of available settings profiles, the first active profile will be picked during "
            + "Game Manager initialization"
        )]
        [SerializeField]
        private List<GameManagerSettingsProfile> profiles = new();

        private IGameManagerSettingsProfile profileOverride;
        private static GameManagerSettings currentSettings;

        /// <summary>
        /// Current <see cref="GameManagerSettings"/> instance.
        /// </summary>
        internal static GameManagerSettings Instance
        {
            get
            {
                InitializeCurrentSettings();
                return currentSettings;
            }
        }

        /// <summary>
        /// Currently active settings instance. Never <c>null</c>.
        /// </summary>
        internal IGameManagerSettingsProfile ActiveProfile
        {
            get => GetActiveProfile();
            set => profileOverride = value;
        }

        /// <summary>
        /// Add a set of new profiles to the <see cref="profiles"/> list. Duplicated profiles will
        /// not be added.
        /// </summary>
        internal void AddProfiles(IEnumerable<GameManagerSettingsProfile> newProfiles)
        {
            foreach (var newProfile in newProfiles)
            {
                if (profiles.Contains(newProfile))
                {
                    continue;
                }

                profiles.Add(newProfile);
            }
        }

        private static void InitializeCurrentSettings()
        {
            if (currentSettings)
            {
                return;
            }

            if (TryGetSettings(out var settings))
            {
                currentSettings = settings;
            }
            else
            {
                Logging.LogWarning(
                    $"Could not find {nameof(GameManagerSettings)}, using defaults",
                    typeof(GameManagerSettings)
                );

                currentSettings = CreateInstance<GameManagerSettings>();
            }
        }

        private static bool TryGetSettings(out GameManagerSettings settings)
        {
            var loadedSettings = Resources
                .FindObjectsOfTypeAll<GameManagerSettings>()
                .FirstOrDefault();

            if (loadedSettings)
            {
                settings = loadedSettings;
                return true;
            }

            settings = default;
            return false;
        }

        private IGameManagerSettingsProfile GetActiveProfile()
        {
            if (profileOverride != default)
            {
                return profileOverride;
            }

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

            return DefaultGameManagerSettingsProfile.Instance;
        }
    }
}
