using System;
using CHARK.GameManagement.Messaging;
using NUnit.Framework;

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
        public void ShouldRegisterAndUnregisterLocalListener()
        {
            // When
            messageBus.AddListener<SimpleTestMessage>(OnSimpleTestMessage);
            messageBus.RemoveListener<SimpleTestMessage>(OnSimpleTestMessage);

            // Then
            Assert.AreEqual(0, messageBus.MessageListenerCount);
            Assert.AreEqual(0, messageBus.TotalListenerCount);
            Assert.AreEqual(0, messageBus.CachedTypeCount);

            return;

            // Given
            void OnSimpleTestMessage(SimpleTestMessage message)
            {
            }
        }

        [Test]
        public void ShouldRegisterAndUnregisterDelegateListener()
        {
            // Given
            OnMessageReceived<SimpleTestMessage> onSimpleTestOnMessage = _ =>
            {
            };

            // When
            messageBus.AddListener(onSimpleTestOnMessage);
            messageBus.RemoveListener(onSimpleTestOnMessage);

            // Then
            Assert.AreEqual(0, messageBus.MessageListenerCount);
            Assert.AreEqual(0, messageBus.TotalListenerCount);
            Assert.AreEqual(0, messageBus.CachedTypeCount);
        }

        [Test]
        public void ShouldRegisterSimpleListenerAndPublishSimpleMessage()
        {
            // Given
            var expectedMessage = new SimpleTestMessage();

            // When
            SimpleTestMessage actualMessage = default;
            messageBus.AddListener<SimpleTestMessage>(
                message => { actualMessage = message; }
            );

            messageBus.Publish(expectedMessage);

            // Then
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void ShouldRegisterAbstractListenerAndPublishConcreteMessage()
        {
            // Given
            var expectedMessage = new ChildTestMessage();

            // When
            BaseTestMessage actualMessage = default;
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
