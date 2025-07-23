using System;
using System.Runtime.CompilerServices;
using CHARK.GameManagement.Settings;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CHARK.GameManagement.Utilities
{
    public readonly struct GameLogger
    {
        private readonly Object context;
        private readonly Type contextType;

        internal GameLogger(Object context)
        {
            this.context = context;
            this.contextType = context.GetType();
        }

        internal GameLogger(Type contextType)
        {
            this.context = null;
            this.contextType = contextType;
        }

        /// <summary>
        /// Verbose log message, use in debug log-spam. Will not print any log messages
        /// if <see cref="GameManagerSettingsProfile.IsVerboseLogging"/> is disabled.
        /// </summary>
        [HideInCallstack]
        public void LogDebug(string message, [CallerLineNumber] int lineNumber = 0)
        {
            var settings = GameManagerSettings.Instance;
            if (settings == false)
            {
                return;
            }

            var profile = settings.ActiveProfile;
            if (profile == null)
            {
                return;
            }

            if (profile.IsVerboseLogging)
            {
                if (context)
                {
                    Debug.Log(
                        FormatLog(message, context.name, lineNumber),
                        context
                    );
                }
                else
                {
                    Debug.Log(FormatLog(message, contextType.Name, lineNumber));
                }
            }
        }

        /// <summary>
        /// Regular log message, use for important/informational log messages.
        /// </summary>
        [HideInCallstack]
        public void LogInfo(string message, [CallerLineNumber] int lineNumber = 0)
        {
            if (context)
            {
                Debug.Log(
                    FormatLog(message, context.name, lineNumber),
                    context
                );
            }
            else
            {
                Debug.Log(FormatLog(message, contextType.Name, lineNumber));
            }
        }

        /// <summary>
        /// Warning log message, use when attention needs to be drawn to a specific
        /// state or in fallback messages.
        /// </summary>
        [HideInCallstack]
        public void LogWarning(string message, [CallerLineNumber] int lineNumber = 0)
        {
            if (context)
            {
                Debug.LogWarning(
                    FormatLog(message, context.name, lineNumber),
                    context
                );
            }
            else
            {
                Debug.LogWarning(FormatLog(message, contextType.Name, lineNumber));
            }
        }

        /// <summary>
        /// Error log message, use when a state is invalid or a component can't function.
        /// </summary>
        [HideInCallstack]
        public void LogError(string message, [CallerLineNumber] int lineNumber = 0)
        {
            if (context)
            {
                Debug.LogError(
                    FormatLog(message, context.name, lineNumber),
                    context
                );
            }
            else
            {
                Debug.LogError(FormatLog(message, contextType.Name, lineNumber));
            }
        }

        /// <summary>
        /// Exception, unrecoverable error/action which cannot be reversed.
        /// </summary>
        [HideInCallstack]
        public void LogError(Exception exception, [CallerLineNumber] int lineNumber = 0)
        {
            if (context)
            {
                Debug.LogException(exception, context);
            }
            else
            {
                Debug.LogException(exception);
            }
        }

        private static string FormatLog(string message, string contextName, int lineNumber)
        {
            return $"[{contextName}:{lineNumber}]: {message}";
        }
    }
}
