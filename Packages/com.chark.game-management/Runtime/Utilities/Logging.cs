using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CHARK.GameManagement.Utilities
{
    /// <summary>
    /// Logging utilities to wrap Unity logging and add some spice.
    /// </summary>
    internal static class Logging
    {
        internal static void LogDebug(string message, Type owner)
        {
#if UNITY_EDITOR
            Debug.Log($"[<b><color=cyan>{owner.Name}</color></b>]: {message}");
#else
            Debug.Log($"[{owner.Name}]: {message}");
#endif
        }

        internal static void LogDebug(string message, Object owner)
        {
            var name = string.IsNullOrWhiteSpace(owner.name)
                ? owner.GetType().Name
                : owner.name;

#if UNITY_EDITOR
            Debug.Log($"[<b><color=cyan>{name}</color></b>]: {message}", owner);
#else
            Debug.Log($"[{name}]: {message}", owner);
#endif
        }

        internal static void LogWarning(string message, Type owner)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"[<b><color=yellow>{owner.Name}</color></b>]: {message}");
#else
            Debug.LogWarning($"[{owner.Name}]: {message}");
#endif
        }

        internal static void LogWarning(string message, Object owner)
        {
            var name = string.IsNullOrWhiteSpace(owner.name)
                ? owner.GetType().Name
                : owner.name;

#if UNITY_EDITOR
            Debug.LogWarning($"[<b><color=orange>{name}</color></b>]: {message}", owner);
#else
            Debug.LogWarning($"[{name}]: {message}", owner);
#endif
        }

        internal static void LogError(string message, Type owner)
        {
#if UNITY_EDITOR
            Debug.LogError($"[<b><color=red>{owner.Name}</color></b>]: {message}");
#else
            Debug.LogError($"[{owner.Name}]: {message}");
#endif
        }

        internal static void LogError(string message, Object owner)
        {
            var name = string.IsNullOrWhiteSpace(owner.name)
                ? owner.GetType().Name
                : owner.name;

#if UNITY_EDITOR
            Debug.LogError($"[<b><color=red>{name}</color></b>]: {message}", owner);
#else
            Debug.LogError($"[{name}]: {message}", owner);
#endif
        }

        internal static void LogException(Exception exception, Type owner)
        {
            Debug.LogException(exception);
        }

        internal static void LogException(Exception exception, Object owner)
        {
            Debug.LogException(exception, owner);
        }
    }
}
