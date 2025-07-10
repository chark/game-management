using CHARK.GameManagement.Messaging;

namespace CHARK.GameManagement
{
    /// <summary>
    /// Raised when <see cref="GameManager.IsDebuggingEnabled"/> changes.
    /// </summary>
    public readonly struct DebuggingChangedMessage : IMessage
    {
        /// <summary>
        /// Debugging mode state the game manager is currently in.
        /// </summary>
        public bool IsDebuggingEnabled { get; }

        public DebuggingChangedMessage(bool isDebuggingEnabledNew)
        {
            IsDebuggingEnabled = isDebuggingEnabledNew;
        }
    }

    /// <summary>
    /// Raised when <see cref="GameManager.IsApplicationQuitting"/> is set to <c>true</c>.
    /// </summary>
    public readonly struct ApplicationQuittingMessage : IMessage
    {
    }
}
