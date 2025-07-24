using System;
using System.Collections.Generic;
using System.Threading;

#if UNITASK_INSTALLED
using AsyncTask = Cysharp.Threading.Tasks.UniTask;
#else
using AsyncTask = System.Threading.Tasks.Task;
#endif

namespace CHARK.GameManagement.Messaging
{
    internal sealed class MessageListener
    {
        private readonly IDictionary<object, Action<IMessage>> wrapperListenersByInstance =
            new Dictionary<object, Action<IMessage>>();

        private readonly IList<Action<IMessage>> wrapperListeners =
            new List<Action<IMessage>>();

        private readonly IDictionary<object, Func<IMessage, CancellationToken, AsyncTask>>
            wrapperAsyncListenersByInstance =
                new Dictionary<object, Func<IMessage, CancellationToken, AsyncTask>>();

        private readonly IList<Func<IMessage, CancellationToken, AsyncTask>> wrapperAsyncListeners =
            new List<Func<IMessage, CancellationToken, AsyncTask>>();

        private readonly List<AsyncTask> raiseAsyncTaskPool = new();

        public int ListenerCount => wrapperListeners.Count;

        public void Raise(IMessage message)
        {
            for (var index = wrapperListeners.Count - 1; index >= 0; index--)
            {
                var wrapperListener = wrapperListeners[index];
                wrapperListener.Invoke(message);
            }
        }

        public async AsyncTask RaiseAsync(
            IMessage message,
            CancellationToken cancellationToken = default
        )
        {
            raiseAsyncTaskPool.Clear();

            for (var index = wrapperAsyncListeners.Count - 1; index >= 0; index--)
            {
                var wrapperListener = wrapperAsyncListeners[index];
                raiseAsyncTaskPool.Add(wrapperListener.Invoke(message, cancellationToken));
            }

            await AsyncTask.WhenAll(raiseAsyncTaskPool);
        }

        public void AddListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage
        {
            if (wrapperListenersByInstance.ContainsKey(listener))
            {
#if UNITY_EDITOR
                GameManager.LogWith(GetType()).LogWarning($"Listener {listener} already added");
#endif
                return;
            }

            var newWrapperListener = new Action<IMessage>(OnMessageReceived);

            wrapperListenersByInstance[listener] = newWrapperListener;
            wrapperListeners.Add(newWrapperListener);

            return;

            void OnMessageReceived(IMessage message)
            {
                try
                {
                    listener.Invoke((TMessage)message);
                }
                catch (Exception exception)
                {
                    GameManager.LogWith(GetType()).LogError(exception);
                }
            }
        }

        public void AddListener<TMessage>(OnMessageReceivedAsync<TMessage> listener) where TMessage : IMessage
        {
            if (wrapperAsyncListenersByInstance.ContainsKey(listener))
            {
#if UNITY_EDITOR
                GameManager.LogWith(GetType()).LogWarning($"Listener {listener} already added");
#endif
                return;
            }

            var newWrapperListener = new Func<IMessage, CancellationToken, AsyncTask>(OnMessageReceivedAsync);

            wrapperAsyncListenersByInstance[listener] = newWrapperListener;
            wrapperAsyncListeners.Add(newWrapperListener);

            return;

            AsyncTask OnMessageReceivedAsync(IMessage message, CancellationToken cancellationToken)
            {
                try
                {
                    return listener.Invoke((TMessage)message, cancellationToken);
                }
                catch (Exception exception)
                {
                    GameManager.LogWith(GetType()).LogError(exception);
                    return AsyncTask.CompletedTask;
                }
            }
        }

        public void RemoveListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage
        {
            if (wrapperListenersByInstance.Remove(listener, out var wrapperListener) == false)
            {
#if UNITY_EDITOR
                GameManager.LogWith(GetType()).LogWarning($"Listener {listener} not added");
#endif
                return;
            }

            wrapperListeners.Remove(wrapperListener);
        }

        public void RemoveListener<TMessage>(OnMessageReceivedAsync<TMessage> listener) where TMessage : IMessage
        {
            if (wrapperAsyncListenersByInstance.Remove(listener, out var wrapperListener) == false)
            {
#if UNITY_EDITOR
                GameManager.LogWith(GetType()).LogWarning($"Listener {listener} not added");
#endif
                return;
            }

            wrapperAsyncListeners.Remove(wrapperListener);
        }
    }
}
