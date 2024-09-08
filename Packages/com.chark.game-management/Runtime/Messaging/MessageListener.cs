using System;
using System.Collections.Generic;
using CHARK.GameManagement.Utilities;

namespace CHARK.GameManagement.Messaging
{
    internal sealed class MessageListener
    {
        private readonly IDictionary<object, Action<IMessage>> wrapperListenersByInstance =
            new Dictionary<object, Action<IMessage>>();

        private readonly IList<Action<IMessage>> wrapperListeners =
            new List<Action<IMessage>>();

        public int ListenerCount => wrapperListeners.Count;

        public void Raise(IMessage message)
        {
            for (var index = wrapperListeners.Count - 1; index >= 0; index--)
            {
                var wrapperListener = wrapperListeners[index];
                try
                {
                    wrapperListener.Invoke(message);
                }
                catch (Exception exception)
                {
                    Logging.LogException(exception, GetType());
                }
            }
        }

        public void AddListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage
        {
            if (wrapperListenersByInstance.TryGetValue(listener, out var wrapperListener))
            {
                wrapperListeners.Add(wrapperListener);
                return;
            }

            var newWrapperListener = new Action<IMessage>(
                message => { listener.Invoke((TMessage)message); }
            );

            wrapperListenersByInstance[listener] = newWrapperListener;
            wrapperListeners.Add(newWrapperListener);
        }

        public void RemoveListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage
        {
            if (wrapperListenersByInstance.TryGetValue(listener, out var wrapperListener) == false)
            {
                return;
            }

            wrapperListenersByInstance.Remove(wrapperListener);
            wrapperListeners.Remove(wrapperListener);
        }
    }
}
