using System.Collections.Generic;
using System.IO;
using System.Linq;
using CHARK.GameManagement.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace CHARK.GameManagement
{
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(GameManagerSettings),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Game Manager Settings",
        order = CreateAssetMenuConstants.BaseOrder
    )]
    public sealed class GameManagerSettings : ScriptableObject, IGameManagerSettings
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("General", Expanded = true)]
#else
        [Header("General")]
#endif
        [FormerlySerializedAs("isActive")]
        [Tooltip(
            "Is this settings profile the active profile? Useful when you have multiple settings files"
        )]
        [SerializeField]
        private bool isActiveSettings;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Instantiation", Expanded = true)]
#else
        [Header("Instantiation")]
#endif
        [Tooltip(
            "Should " + nameof(GameManager) + " automatically instantiate?"
        )]
        [SerializeField]
        private bool isInstantiateAutomatically;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Instantiation", Expanded = true)]
        [Sirenix.OdinInspector.AssetsOnly]
        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.ShowIf(nameof(isInstantiateAutomatically))]
#endif
        [Tooltip(
            ""
            + "Prefab to instantiate when " + nameof(isInstantiateAutomatically)
            + " is set to true"
        )]
        [SerializeField]
        private GameManager gameManagerPrefab;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Instantiation", Expanded = true)]
        [Sirenix.OdinInspector.ShowIf(nameof(isInstantiateAutomatically))]
#endif
        [Tooltip(
            ""
            + "When to instantiate " + nameof(gameManagerPrefab)
            + " on game start using " + nameof(RuntimeInitializeOnLoadMethodAttribute)
        )]
        [SerializeField]
        private RuntimeInitializeLoadType loadType =
            RuntimeInitializeLoadType.BeforeSceneLoad;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Features", Expanded = true)]
#else
        [Header("Features")]
#endif
        [Tooltip("Should " + nameof(GameManager) + " persist between scene loads?")]
        [SerializeField]
        private bool isDontDestroyOnLoad = true;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Features", Expanded = true)]
#endif
        [Tooltip("Should verbose " + nameof(GameManager) + " logging be enabled?")]
        [SerializeField]
        private bool isVerboseLogging;

        private const string DefaultSettingsPath = "Assets/Resources/GameManagerSettings.asset";

        private static GameManagerSettings currentSettings;

        public bool IsInstantiateAutomatically => isInstantiateAutomatically;

        public RuntimeInitializeLoadType LoadType => loadType;

        public bool IsDontDestroyOnLoad => isDontDestroyOnLoad;

        public bool IsVerboseLogging => isVerboseLogging;

        /// <summary>
        /// Currently active settings instance. Never <c>null</c>.
        /// </summary>
        internal static IGameManagerSettings Instance
        {
            get
            {
                if (currentSettings && currentSettings.IsActiveSettings)
                {
                    return currentSettings;
                }

                if (TryGetSettings(out var settings))
                {
                    currentSettings = settings;
                    currentSettings.IsActiveSettings = true;
                }
                else
                {
                    currentSettings = CreateSettings();
                    currentSettings.IsActiveSettings = true;
                }

                return currentSettings;
            }
        }

        private bool IsActiveSettings
        {
            get => isActiveSettings;
            set
            {
                var isActiveOld = isActiveSettings;
                var isActiveNew = value;

#if UNITY_EDITOR
                if (isActiveOld != isActiveNew)
                {
                    UnityEditor.EditorUtility.SetDirty(this);
                }
#endif

                isActiveSettings = isActiveNew;
            }
        }

        public bool TryGetGameManagerPrefab(out GameManager prefab)
        {
            if (gameManagerPrefab)
            {
                prefab = gameManagerPrefab;
                return true;
            }

            prefab = default;
            return false;
        }

        private static GameManagerSettings CreateSettings()
        {
            var settings = CreateInstance<GameManagerSettings>();

#if UNITY_EDITOR
            var directoryName = Path.GetDirectoryName(DefaultSettingsPath);
            if (Directory.Exists(directoryName) == false)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Directory.CreateDirectory(directoryName);
            }

            UnityEditor.AssetDatabase.CreateAsset(
                settings,
                DefaultSettingsPath
            );

            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            return UnityEditor.AssetDatabase.LoadAssetAtPath<GameManagerSettings>(
                DefaultSettingsPath
            );
#else
            return settings;
#endif
        }

        private static bool TryGetSettings(out GameManagerSettings settings)
        {
            var gameManagerList = GetGamerManagers().ToList();

            var newSettings = gameManagerList.FirstOrDefault(settings => settings.IsActiveSettings);
            if (newSettings == false)
            {
                newSettings = gameManagerList.FirstOrDefault();
            }

            if (newSettings)
            {
                settings = newSettings;
                return true;
            }

            settings = default;
            return false;
        }

        private static IEnumerable<GameManagerSettings> GetGamerManagers()
        {
            return Resources.FindObjectsOfTypeAll<GameManagerSettings>();
        }
    }
}
