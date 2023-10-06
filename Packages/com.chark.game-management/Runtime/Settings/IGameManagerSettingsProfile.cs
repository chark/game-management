﻿using UnityEngine;

namespace CHARK.GameManagement.Settings
{
    internal interface IGameManagerSettingsProfile
    {
        /// <summary>
        /// Profile name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Should <see cref="GameManager"/> prefab instantiate automatically?
        /// </summary>
        public bool IsInstantiateAutomatically { get; }

        /// <summary>
        /// When to load <see cref="GameManager"/> prefab when instantiating it automatically. Used
        /// only when <see cref="IsInstantiateAutomatically"/> is set to <c>true</c>.
        /// </summary>
        public InstantiationMode InstantiationMode { get; }

        /// <summary>
        /// Should <see cref="GameManager"/> use <see cref="Object.DontDestroyOnLoad"/>.
        /// </summary>
        public bool IsDontDestroyOnLoad { get; }

        /// <summary>
        /// Should logging be enabled?
        /// </summary>
        public bool IsVerboseLogging { get; }

        /// <returns>
        /// <c>true</c> if <paramref name="prefab"/> is retrieved or <c>false</c> otherwise.
        /// </returns>
        public bool TryGetGameManagerPrefab(out GameManager prefab);
    }
}
