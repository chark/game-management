namespace CHARK.GameManagement.Messaging
{
    public delegate void OnMessageReceived<in TMessage>(TMessage message) where TMessage : IMessage;
}
