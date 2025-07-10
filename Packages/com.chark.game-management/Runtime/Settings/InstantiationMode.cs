using UnityEngine;

namespace CHARK.GameManagement.Settings
{
    internal enum InstantiationMode
    {
        /// <summary>
        /// <see cref="GameManager"/> is instantiated before when setting up the runtime.
        /// For more info <see cref="RuntimeInitializeLoadType.SubsystemRegistration"/>.
        /// </summary>
        SubsystemRegistration = 0,

        /// <summary>
        /// <see cref="GameManager"/> is instantiated when all assemblies are loaded.
        /// For more info <see cref="RuntimeInitializeLoadType.AfterAssembliesLoaded"/>.
        /// </summary>
        AfterAssembliesLoaded = 1,

        /// <summary>
        /// <see cref="GameManager"/> is instantiated before the splash shows.
        /// For more info <see cref="RuntimeInitializeLoadType.BeforeSplashScreen"/>.
        /// </summary>
        BeforeSplashScreen = 2,

        /// <summary>
        /// <see cref="GameManager"/> is instantiated before the scene loads.
        /// For more info <see cref="RuntimeInitializeLoadType.BeforeSceneLoad"/>.
        /// </summary>
        BeforeSceneLoad = 3,

        /// <summary>
        /// <see cref="GameManager"/> is instantiated after the scene loads.
        /// For more info <see cref="RuntimeInitializeLoadType.AfterSceneLoad"/>.
        /// </summary>
        AfterSceneLoad = 4,
    }
}
