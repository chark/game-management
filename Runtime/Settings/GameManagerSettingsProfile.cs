using CHARK.GameManagement.Utilities;
using UnityEngine;

namespace CHARK.GameManagement.Settings
{
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(GameManagerSettingsProfile),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Game Manager Settings Profile",
        order = CreateAssetMenuConstants.BaseOrder
    )]
    internal sealed class GameManagerSettingsProfile : ScriptableObject, IGameManagerSettingsProfile
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("General", Expanded = true)]
#else
        [Header("General")]
#endif
        [Tooltip(
            "Is this settings profile the active profile?"
        )]
        [SerializeField]
        private bool isActiveProfile;

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
        private InstantiationMode instantiationMode = InstantiationMode.BeforeSceneLoad;

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

        internal bool IsActiveProfile
        {
            get => isActiveProfile;
            set
            {
                var isActiveOld = isActiveProfile;
                var isActiveNew = value;

#if UNITY_EDITOR
                if (isActiveOld != isActiveNew)
                {
                    UnityEditor.EditorUtility.SetDirty(this);
                }
#endif

                isActiveProfile = value;
            }
        }

        public bool IsInstantiateAutomatically => isInstantiateAutomatically;

        public InstantiationMode InstantiationMode => instantiationMode;

        public bool IsDontDestroyOnLoad => isDontDestroyOnLoad;

        public bool IsVerboseLogging => isVerboseLogging;

        public bool TryGetGameManagerPrefab(out GameManager prefab)
        {
            if (gameManagerPrefab)
            {
                prefab = gameManagerPrefab;
                return true;
            }

            prefab = default;
            return true;
        }
    }
}
