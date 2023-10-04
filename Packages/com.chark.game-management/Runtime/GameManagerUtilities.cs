using UnityEngine;

namespace CHARK.GameManagement
{
    internal static class GameManagerUtilities
    {
        /// <summary>
        /// <c>true</c> if application is about to quit or <c>false</c> otherwise.
        /// </summary>
        public static bool IsApplicationQuitting { get; private set; }

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod]
        private static void InitializeApplicationListeners()
        {
            Application.quitting += OnApplicationQuitting;
        }

        private static void OnApplicationQuitting()
        {
            IsApplicationQuitting = true;
        }
#endif
    }
}
