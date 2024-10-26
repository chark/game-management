using CHARK.GameManagement.Actors;
using CHARK.GameManagement.Messaging;

namespace CHARK.GameManagement
{
    /// <summary>
    /// Raised when <see cref="GameManager.IsDebuggingEnabled"/> changes.
    /// </summary>
    public readonly struct DebuggingChangedMessage : IMessage
    {
        /// <summary>
        /// Debugging mode state the game manager transitioned from.
        /// </summary>
        public bool IsDebuggingEnabledPrev { get; }

        /// <summary>
        /// Debugging mode state the game manager is currently in.
        /// </summary>
        public bool IsDebuggingEnabled { get; }

        public DebuggingChangedMessage(bool isDebuggingEnabledPrev, bool isDebuggingEnabledNew)
        {
            IsDebuggingEnabledPrev = isDebuggingEnabledPrev;
            IsDebuggingEnabled = isDebuggingEnabledNew;
        }
    }

    public readonly struct ActorAddedMessage : IMessage
    {
        /// <summary>
        /// Actor that was added to the game manager.
        /// </summary>
        public IActor Actor { get; }

        public ActorAddedMessage(IActor actor)
        {
            Actor = actor;
        }
    }

    public readonly struct ActorRemovedMessage : IMessage
    {
        /// <summary>
        /// Actor that was removed from the game manager.
        /// </summary>
        public IActor Actor { get; }

        public ActorRemovedMessage(IActor actor)
        {
            Actor = actor;
        }
    }
}
