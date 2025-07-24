using System;
using System.Threading;
using System.Threading.Tasks;
using CHARK.GameManagement.Messaging;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

#if UNITASK_INSTALLED
using AsyncTask = Cysharp.Threading.Tasks.UniTask;
#else
using AsyncTask = System.Threading.Tasks.Task;
#endif

namespace CHARK.GameManagement.Tests.Editor
{
    internal sealed class DefaultMessageBusTest
    {
        private DefaultMessageBus messageBus;

        [SetUp]
        public void SetUp()
        {
            messageBus = new DefaultMessageBus();
        }

        [Test]
        public void ShouldRegisterListener()
        {
            // Given
            OnMessageReceived<SimpleTestMessage> onSimpleTestOnMessage = _ => { };

            // When
            messageBus.AddListener(onSimpleTestOnMessage);

            // Then
            Assert.AreEqual(1, messageBus.MessageListenerCount);
            Assert.AreEqual(1, messageBus.TotalListenerCount);
            Assert.AreEqual(1, messageBus.CachedTypeCount);
        }

        [Test]
        public void ShouldRegisterAndUnregisterListener()
        {
            // Given
            OnMessageReceived<SimpleTestMessage> onSimpleTestOnMessage = _ => { };

            // When
            messageBus.AddListener(onSimpleTestOnMessage);
            messageBus.RemoveListener(onSimpleTestOnMessage);

            // Then
            Assert.AreEqual(0, messageBus.MessageListenerCount);
            Assert.AreEqual(0, messageBus.TotalListenerCount);
            Assert.AreEqual(0, messageBus.CachedTypeCount);
        }

        [Test]
        public void ShouldRegisterAndUnregisterAsyncListeners()
        {
            // Given
            OnMessageReceivedAsync<SimpleTestMessage> onMessageAsync = _ => AsyncTask.CompletedTask;
            OnMessageReceivedCancellableAsync<SimpleTestMessage> onMessageCancellableAsync = (_, _) => AsyncTask.CompletedTask;

            // When
            messageBus.AddListener(onMessageAsync);
            messageBus.AddListener(onMessageCancellableAsync);

            messageBus.RemoveListener(onMessageAsync);
            messageBus.RemoveListener(onMessageCancellableAsync);

            // Then
            Assert.AreEqual(0, messageBus.MessageListenerCount);
            Assert.AreEqual(0, messageBus.TotalListenerCount);
            Assert.AreEqual(0, messageBus.CachedTypeCount);
        }

        [Test]
        public void ShouldRegisterListenersOnce()
        {
            // Given
            OnMessageReceived<SimpleTestMessage> onSimpleTestOnMessage = _ => { };

            // When
            messageBus.AddListener(onSimpleTestOnMessage);
            messageBus.AddListener(onSimpleTestOnMessage);

            // Then
            Assert.AreEqual(1, messageBus.MessageListenerCount);
            Assert.AreEqual(1, messageBus.TotalListenerCount);
            Assert.AreEqual(1, messageBus.CachedTypeCount);
        }

        [Test]
        public void ShouldRegisterListenerAndPublishSimpleMessage()
        {
            // Given
            var expectedMessage = new SimpleTestMessage();

            // When
            SimpleTestMessage actualMessage = null;
            messageBus.AddListener<SimpleTestMessage>(
                message => { actualMessage = message; }
            );

            messageBus.Publish(expectedMessage);

            // Then
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void ShouldContinueRaisingOtherListenersIfOneFails()
        {
            // Given
            var expectedMessage = new SimpleTestMessage();

            // When
            SimpleTestMessage actualMessage = null;

            messageBus.AddListener<SimpleTestMessage>(_ => throw new Exception("fail"));
            messageBus.AddListener<SimpleTestMessage>(message => actualMessage = message);

            // Then
            LogAssert.Expect(LogType.Exception, "Exception: fail");
            Assert.DoesNotThrow(() => messageBus.Publish(expectedMessage));
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public async Task ShouldRegisterListenerAndPublishAsyncMessage()
        {
            // Given
            var expectedMessage = new SimpleTestMessage();

            // When
            SimpleTestMessage actualMessage = null;
            messageBus.AddListener<SimpleTestMessage>((message, _) =>
                {
                    actualMessage = message;
                    return AsyncTask.CompletedTask;
                }
            );

            await messageBus.PublishAsync(expectedMessage, CancellationToken.None);

            // Then
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public async Task ShouldRegisterListenerAndPublishAsyncDelayedMessages()
        {
            // Given
            var expectedMessage = new SimpleTestMessage();

            // When
            SimpleTestMessage actualMessageA = null;
            SimpleTestMessage actualMessageB = null;

            messageBus.AddListener<SimpleTestMessage>(async (message, token) =>
                {
                    await AsyncTask.Delay(TimeSpan.FromMilliseconds(10), cancellationToken: token);
                    actualMessageA = message;
                }
            );

            messageBus.AddListener<SimpleTestMessage>(async (message, token) =>
                {
                    await AsyncTask.Delay(TimeSpan.FromMilliseconds(20), cancellationToken: token);
                    actualMessageB = message;
                }
            );

            await messageBus.PublishAsync(expectedMessage, CancellationToken.None);

            // Then
            Assert.AreEqual(expectedMessage, actualMessageA);
            Assert.AreEqual(expectedMessage, actualMessageB);
        }

        [Test]
        public async Task ShouldRegisterSyncAndAsyncListenersAndPublishSyncMessage()
        {
            // Given
            var expectedMessage = new SimpleTestMessage();

            // When
            SimpleTestMessage actualMessageA = null;
            SimpleTestMessage actualMessageB = null;

            messageBus.AddListener<SimpleTestMessage>(message =>
                {
                    actualMessageA = message;
                }
            );

            messageBus.AddListener<SimpleTestMessage>((message, _) =>
                {
                    actualMessageB = message;
                    return AsyncTask.CompletedTask;
                }
            );

            // ReSharper disable once MethodHasAsyncOverload
            messageBus.Publish(expectedMessage);

            await AsyncTask.Delay(TimeSpan.FromMilliseconds(10));

            // Then
            Assert.AreEqual(expectedMessage, actualMessageA);
            Assert.AreEqual(expectedMessage, actualMessageB);
        }

        [Test]
        public async Task ShouldRegisterSyncAndAsyncListenersAndPublishAsyncMessage()
        {
            // Given
            var expectedMessage = new SimpleTestMessage();

            // When
            SimpleTestMessage actualMessageA = null;
            SimpleTestMessage actualMessageB = null;

            messageBus.AddListener<SimpleTestMessage>(message =>
                {
                    actualMessageA = message;
                }
            );

            messageBus.AddListener<SimpleTestMessage>((message, _) =>
                {
                    actualMessageB = message;
                    return AsyncTask.CompletedTask;
                }
            );

            await messageBus.PublishAsync(expectedMessage, cancellationToken: CancellationToken.None);

            // Then
            Assert.AreEqual(expectedMessage, actualMessageA);
            Assert.AreEqual(expectedMessage, actualMessageB);
        }

        [Test]
        public async Task ShouldRegisterListenerAndPublishAndCancelAsyncMessage()
        {
            // Given
            var expectedMessage = new SimpleTestMessage();

            // When
            SimpleTestMessage actualMessageA = null;
            SimpleTestMessage actualMessageB = null;

            messageBus.AddListener<SimpleTestMessage>(async (message, token) =>
                {
                    await AsyncTask.Delay(TimeSpan.FromMilliseconds(100), cancellationToken: token);
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    actualMessageA = message;
                }
            );

            messageBus.AddListener<SimpleTestMessage>(async (message, token) =>
                {
                    await AsyncTask.Delay(TimeSpan.FromMilliseconds(110), cancellationToken: token);
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    actualMessageB = message;
                }
            );

            var tokenSource = new CancellationTokenSource();
            var publishTask = messageBus.PublishAsync(expectedMessage, tokenSource.Token);

            tokenSource.Cancel();
            tokenSource.Dispose();

            await publishTask;

            // Then
            Assert.IsNull(actualMessageA);
            Assert.IsNull(actualMessageB);
        }

        [Test]
        public void ShouldRegisterAbstractListenerAndPublishConcreteMessage()
        {
            // Given
            var expectedMessage = new ChildTestMessage();

            // When
            BaseTestMessage actualMessage = null;
            messageBus.AddListener<BaseTestMessage>(
                publishedMessage => { actualMessage = publishedMessage; }
            );

            messageBus.Publish(expectedMessage);

            // Then
            Assert.IsNotNull(actualMessage);
            Assert.IsAssignableFrom<ChildTestMessage>(actualMessage);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void ShouldRegisterInterfaceListenerAndPublishConcreteMessage()
        {
            // Given
            var expectedMessage = new ChildTestMessage();

            // When
            ITestMessage actualMessage = default;
            messageBus.AddListener<ITestMessage>(
                publishedMessage => { actualMessage = publishedMessage; }
            );

            messageBus.Publish(expectedMessage);

            // Then
            Assert.IsNotNull(actualMessage);
            Assert.IsAssignableFrom<ChildTestMessage>(actualMessage);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void ShouldReceiveMessagesWhenRemovingListenersDuringProcessing()
        {
            // Given
            var listener01 = new SimpleMessageTestController(messageBus);
            listener01.Initialize();
            listener01.OnSimpleMessageReceived += () => listener01.Dispose();

            var listener02 = new SimpleMessageTestController(messageBus);
            listener02.Initialize();
            listener02.OnSimpleMessageReceived += () => listener02.Dispose();

            var listener03 = new SimpleMessageTestController(messageBus);
            listener03.Initialize();
            listener03.OnSimpleMessageReceived += () => listener03.Dispose();

            // When
            messageBus.Publish(new SimpleTestMessage());

            // Then
            Assert.AreEqual(1, listener01.ReceivedMessageCount);
            Assert.AreEqual(1, listener02.ReceivedMessageCount);
            Assert.AreEqual(1, listener03.ReceivedMessageCount);
        }

        private class SimpleMessageTestController
        {
            private readonly IMessageBus messageBus;

            public int ReceivedMessageCount { get; private set; }

            public event Action OnSimpleMessageReceived;

            public SimpleMessageTestController(IMessageBus messageBus)
            {
                this.messageBus = messageBus;
            }

            public void Initialize()
            {
                messageBus.AddListener<SimpleTestMessage>(OnSimpleMessage);
            }

            public void Dispose()
            {
                messageBus.RemoveListener<SimpleTestMessage>(OnSimpleMessage);
            }

            private void OnSimpleMessage(SimpleTestMessage message)
            {
                ReceivedMessageCount++;
                OnSimpleMessageReceived?.Invoke();
            }
        }

        private sealed class SimpleTestMessage : IMessage
        {
        }

        private sealed class ChildTestMessage : BaseTestMessage
        {
        }

        private abstract class BaseTestMessage : ITestMessage
        {
        }

        private interface ITestMessage : IMessage
        {
        }
    }
}
