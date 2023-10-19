using System;
using System.Collections.Generic;
using CHARK.GameManagement.Utilities;

namespace CHARK.GameManagement.Messaging
{
    internal sealed class MessageListener<TMessage> : MessageListener where TMessage : IMessage
    {
        private readonly IList<Action<TMessage>> listeners = new List<Action<TMessage>>();

        public int ListenerCount => listeners.Count;

        public void Raise(TMessage message)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < listeners.Count; index++)
            {
                var listener = listeners[index];
                try
                {
                    listener.Invoke(message);
                }
                catch (Exception exception)
                {
                    Logging.LogException(exception, GetType());
                }
            }
        }

        public void AddListener(Action<TMessage> listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(Action<TMessage> listener)
        {
            listeners.Remove(listener);
        }
    }

    internal abstract class MessageListener
    {
    }
}
