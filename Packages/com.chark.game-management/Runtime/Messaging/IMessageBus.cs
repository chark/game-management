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

        /// <summary>
        /// Add a new <paramref name="listener"/> which listens for incoming messages of type
        /// <see cref="TMessage"/>.
        /// </summary>
        public void AddListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage;

        /// <summary>
        /// Remove existing <paramref name="listener"/> of type <see cref="TMessage"/>.
        /// </summary>
        public void RemoveListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage;
    }
}
