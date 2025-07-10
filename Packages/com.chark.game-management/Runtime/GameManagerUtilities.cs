using UnityEngine;

namespace CHARK.GameManagement
{
    internal static class GameManagerUtilities
    {
        private static bool isApplicationQuitting;

        /// <summary>
        /// <c>true</c> if application is about to quit or <c>false</c> otherwise.
        /// </summary>
        public static bool IsApplicationQuitting
        {
            get
            {
#if UNITY_EDITOR
                if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode == false)
                {
                    return true;
                }
#endif

                return isApplicationQuitting;
            }
            private set
            {
                var prevValue = isApplicationQuitting;
                var nextValue = value;

                if (prevValue == nextValue)
                {
                    return;
                }

                isApplicationQuitting = nextValue;

                if (nextValue)
                {
                    GameManager.Publish(new ApplicationQuittingMessage());
                }
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void InitializeApplicationListeners()
        {
            IsApplicationQuitting = false;

            Application.quitting -= OnApplicationQuitting;
            Application.quitting += OnApplicationQuitting;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }

        private static void OnApplicationQuitting()
        {
            IsApplicationQuitting = true;
        }

#if UNITY_EDITOR
        private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                IsApplicationQuitting = true;
            }
        }
#endif
    }
}
