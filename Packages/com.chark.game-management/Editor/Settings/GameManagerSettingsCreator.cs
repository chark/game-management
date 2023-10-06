using System.Collections.Generic;
using System.IO;
using System.Linq;
using CHARK.GameManagement.Settings;
using CHARK.GameManagement.Utilities;
using UnityEditor;
using UnityEngine;

namespace CHARK.GameManagement.Editor.Settings
{
    /// <summary>
    /// Creates game manager settings assets if they're missing and adds them to preloaded assets.
    /// By using this approach, <see cref="GameManagerSettings"/> can be loaded using
    /// <see cref="Resources.FindObjectsOfTypeAll{T}"/> during runtime (and in builds).
    /// </summary>
    internal static class GameManagerSettingsCreator
    {
        private const string DefaultSettingsAssetPath = "Assets/Settings/GameManagerSettings.asset";

        [InitializeOnLoadMethod]
        private static void OnInitializeEditor()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            UpdatePreloadedAssets();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                // We also need to update on play-mode change, otherwise assets added to preloaded
                // list after hitting the play button will not persist.
                UpdatePreloadedAssets();
            }
        }

        private static void UpdatePreloadedAssets()
        {
            if (IsSettingsAssetPreloaded())
            {
                return;
            }

            if (TrgGetSettingsAsset(out var existingAsset))
            {
                PreloadSettingsAsset(existingAsset);
            }
            else
            {
                var newAsset = CreateSettingsAsset();
                PreloadSettingsAsset(newAsset);
            }
        }

        private static bool IsSettingsAssetPreloaded()
        {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets();
            foreach (var preloadedAsset in preloadedAssets)
            {
                if (preloadedAsset && preloadedAsset is GameManagerSettings)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TrgGetSettingsAsset(out GameManagerSettings asset)
        {
            var existingAsset = AssetDatabase
                .FindAssets($"t:{nameof(GameManagerSettings)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<GameManagerSettings>)
                .FirstOrDefault();

            if (existingAsset)
            {
                asset = existingAsset;
                return true;
            }

            asset = default;
            return false;
        }

        private static void PreloadSettingsAsset(GameManagerSettings asset)
        {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            if (preloadedAssets.Contains(asset))
            {
                return;
            }

            preloadedAssets.Add(asset);

            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }

        private static GameManagerSettings CreateSettingsAsset()
        {
            var settings = ScriptableObject.CreateInstance<GameManagerSettings>();
            settings.AddProfiles(GetAvailableProfilesEditor());

            var directoryName = Path.GetDirectoryName(DefaultSettingsAssetPath);
            if (string.IsNullOrWhiteSpace(directoryName) == false && Directory.Exists(directoryName) == false)
            {
                Directory.CreateDirectory(directoryName);
            }

            AssetDatabase.CreateAsset(settings, DefaultSettingsAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Logging.LogDebug(
                ""
                + $"Created new {nameof(GameManagerSettings)} at {DefaultSettingsAssetPath},"
                + $" you can move this asset to a any directory and rename it as you'd like."
                + $" <b>Make sure to add {nameof(GameManagerSettingsProfile)} to this asset!</b>",
                settings
            );

            return settings;
        }

        private static IEnumerable<GameManagerSettingsProfile> GetAvailableProfilesEditor()
        {
            return AssetDatabase
                .FindAssets($"t:{nameof(GameManagerSettingsProfile)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<GameManagerSettingsProfile>);
        }
    }
}
