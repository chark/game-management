﻿using System;
using System.Collections.Generic;
using CHARK.GameManagement.Utilities;

namespace CHARK.GameManagement.Messaging
{
    internal sealed class DefaultMessageBus : IMessageBus
    {
        private readonly IDictionary<Type, Type[]> interfacesByListenerTypeCache =
            new Dictionary<Type, Type[]>();

        private readonly IDictionary<Type, MessageListener> listenersByType =
            new Dictionary<Type, MessageListener>();

        public int CachedTypeCount => interfacesByListenerTypeCache.Count;

        public int MessageListenerCount => listenersByType.Count;

        public int TotalListenerCount
        {
            get
            {
                var totalListenerCount = 0;
                foreach (var messageListener in listenersByType.Values)
                {
                    totalListenerCount += messageListener.ListenerCount;
                }

                return totalListenerCount;
            }
        }

        public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
#if UNITY_EDITOR
            if (message == null)
            {
                Logging.LogError($"Message of type {typeof(TMessage)} cannot be null", GetType());
                return;
            }
#endif

            if (TryGetListener<TMessage>(out var messageListener) == false)
            {
#if UNITY_EDITOR
                Logging.LogWarning($"Could not find a listener for {typeof(TMessage)}", GetType());
#endif
                return;
            }

            messageListener.Raise(message);
        }

        public void AddListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage
        {
#if UNITY_EDITOR
            if (listener == null)
            {
                Logging.LogError($"Listener of type {typeof(TMessage)} cannot be null", GetType());
                return;
            }
#endif

            var listenerType = typeof(TMessage);
            if (TryGetListener<TMessage>(out var messageListener))
            {
                messageListener.AddListener(listener);
                return;
            }

            var newMessageListener = new MessageListener();
            newMessageListener.AddListener(listener);

            listenersByType[listenerType] = newMessageListener;
        }

        public void RemoveListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage
        {
#if UNITY_EDITOR
            if (listener == null)
            {
                Logging.LogError($"Listener of type {typeof(TMessage)} cannot be null", GetType());
                return;
            }
#endif

            var listenerType = typeof(TMessage);
            if (TryGetListener<TMessage>(out var messageListener) == false)
            {
#if UNITY_EDITOR
                Logging.LogWarning($"Could not find a listener for {typeof(TMessage)}", GetType());
#endif
                return;
            }

            messageListener.RemoveListener(listener);

            if (messageListener.ListenerCount == 0)
            {
                interfacesByListenerTypeCache.Remove(listenerType);
                listenersByType.Remove(listenerType);
            }
        }

        private bool TryGetListener<TMessage>(out MessageListener listener) where TMessage : IMessage
        {
            var listenerType = typeof(TMessage);

            // Simple base type listener, the most common case
            {
                if (listenersByType.TryGetValue(listenerType, out listener))
                {
                    return true;
                }
            }

            // Look up base types (abstract classes and such)
            {
                var listenerBaseType = listenerType.BaseType;
                while (listenerBaseType != null && typeof(IMessage).IsAssignableFrom(listenerBaseType))
                {
                    if (listenersByType.TryGetValue(listenerBaseType, out listener))
                    {
                        return true;
                    }

                    listenerBaseType = listenerBaseType.BaseType;
                }
            }

            // Look up interfaces
            {
                // ReSharper disable once InlineOutVariableDeclaration
                Type[] interfaceTypes;

                if (interfacesByListenerTypeCache.TryGetValue(listenerType, out interfaceTypes) == false)
                {
                    interfaceTypes = listenerType.GetInterfaces();
                    interfacesByListenerTypeCache[listenerType] = interfaceTypes;
                }

                // ReSharper disable once ForCanBeConvertedToForeach
                for (var index = 0; index < interfaceTypes.Length; index++)
                {
                    var interfaceType = interfaceTypes[index];
                    if (typeof(IMessage).IsAssignableFrom(interfaceType) == false)
                    {
                        continue;
                    }

                    if (listenersByType.TryGetValue(interfaceType, out listener))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
