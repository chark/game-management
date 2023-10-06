using System;
using System.Collections.Generic;
using CHARK.GameManagement.Utilities;
using UnityEngine;

namespace CHARK.GameManagement.Messaging
{
    internal sealed class MessageBus : IMessageBus
    {
        private readonly IDictionary<Type, MessageListener> listenersByType =
            new Dictionary<Type, MessageListener>();

        public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
#if UNITY_EDITOR
            if (message == null)
            {
                Logging.LogError($"Message of type {typeof(TMessage)} cannot be null", GetType());
                return;
            }
#endif

            var listenerType = message.GetType();
            if (listenersByType.TryGetValue(listenerType, out var messageListener) == false)
            {
                return;
            }

            var typedMessageListener = (MessageListener<TMessage>) messageListener;
            typedMessageListener.Raise(message);
        }

        public void AddListener<TMessage>(Action<TMessage> listener) where TMessage : IMessage
        {
#if UNITY_EDITOR
            if (listener == null)
            {
                Logging.LogError($"Listener of type {typeof(TMessage)} cannot be null", GetType());
                return;
            }
#endif

            var listenerType = typeof(TMessage);
            if (listenersByType.TryGetValue(listenerType, out var messageListener))
            {
                var existingMessageListener = (MessageListener<TMessage>) messageListener;
                existingMessageListener.AddListener(listener);
                return;
            }

            var newMessageListener = new MessageListener<TMessage>();
            newMessageListener.AddListener(listener);

            listenersByType[listenerType] = newMessageListener;
        }

        public void RemoveListener<TMessage>(Action<TMessage> listener) where TMessage : IMessage
        {
#if UNITY_EDITOR
            if (listener == null)
            {
                Logging.LogError($"Listener of type {typeof(TMessage)} cannot be null", GetType());
                return;
            }
#endif

            var listenerType = typeof(TMessage);
            if (listenersByType.TryGetValue(listenerType, out var messageListener) == false)
            {
                return;
            }

            var existingMessageListener = (MessageListener<TMessage>) messageListener;
            existingMessageListener.RemoveListener(listener);

            if (existingMessageListener.ListenerCount == 0)
            {
                listenersByType.Remove(listenerType);
            }
        }
    }
}
