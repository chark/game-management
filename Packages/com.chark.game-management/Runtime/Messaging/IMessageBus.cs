using System.Threading;

#if UNITASK_INSTALLED
using AsyncTask = Cysharp.Threading.Tasks.UniTask;
#else
using AsyncTask = System.Threading.Tasks.Task;
#endif

namespace CHARK.GameManagement.Messaging
{
    /// <summary>
    /// Core application event messaging system.
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// Publish a <paramref name="message"/> and invoke all listeners which are listening for
        /// messages of type <see cref="TMessage"/>.
        /// </summary>
        public void Publish<TMessage>(TMessage message) where TMessage : IMessage;

        public AsyncTask PublishAsync<TMessage>(
            TMessage message,
            CancellationToken cancellationToken = default
        ) where TMessage : IMessage;

        /// <summary>
        /// Add a new <paramref name="listener"/> which listens for incoming messages of type
        /// <see cref="TMessage"/>.
        /// </summary>
        public void AddListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage;

        /// <summary>
        /// Add a new async <paramref name="listener"/> which listens for incoming messages of type
        /// <see cref="TMessage"/>.
        /// </summary>
        public void AddListener<TMessage>(OnMessageReceivedAsync<TMessage> listener) where TMessage : IMessage;

        /// <summary>
        /// Remove existing <paramref name="listener"/> of type <see cref="TMessage"/>.
        /// </summary>
        public void RemoveListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage;

        /// <summary>
        /// Remove existing async <paramref name="listener"/> of type <see cref="TMessage"/>.
        /// </summary>
        public void RemoveListener<TMessage>(OnMessageReceivedAsync<TMessage> listener) where TMessage : IMessage;
    }
}
