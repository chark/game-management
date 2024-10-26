using System;
using System.Linq;
using CHARK.GameManagement.Actors;
using NUnit.Framework;

namespace CHARK.GameManagement.Tests.Editor
{
    internal sealed class DefaultActorManagerTest
    {
        private DefaultActorManager manager;

        [SetUp]
        public void SetUp()
        {
            manager = new DefaultActorManager(GameManagerTestProfile.Instance);
        }

        [Test]
        public void ShouldTryGetActorAndFail()
        {
            if (manager.TryGetActor<TestActor>(out _))
            {
                Assert.Fail("No actors should be added");
            }
        }


        [Test]
        public void ShouldGetActorAndFail()
        {
            var exception = Assert.Throws<Exception>(() => manager.GetActor<TestActor>());
            Assert.AreEqual($"Actor {nameof(TestActor)} is not added to the manager", exception.Message);
        }

        [Test]
        public void ShouldAddActor()
        {
            // Given
            var expectedActor = new TestActor();

            // When
            var isAdded = manager.AddActor(expectedActor);

            // Then
            var expectedActors = new[] { expectedActor };
            var actualActors = manager.Actors;

            Assert.IsTrue(isAdded);

            CollectionAssert.AreEqual(expectedActors, actualActors);
        }

        [Test]
        public void ShouldAddActorOnce()
        {
            // Given
            var expectedActor = new TestActor();

            // When
            var isAddedInitially = manager.AddActor(expectedActor);
            var isAddedTwice = manager.AddActor(expectedActor);

            // Then
            var expectedActors = new[] { expectedActor };
            var actualActors = manager.Actors;

            Assert.IsTrue(isAddedInitially);
            Assert.IsFalse(isAddedTwice);

            CollectionAssert.AreEqual(expectedActors, actualActors);
        }

        [Test]
        public void ShouldAddAndRetrieveActors()
        {
            // Given
            var expectedActor1 = new TestActor();
            var expectedActor2 = new TestActor();

            manager.AddActor(expectedActor1);
            manager.AddActor(expectedActor2);

            // When
            var actualActors = manager.GetActors<TestActor>().ToList();

            // Then
            var expectedActors = new[] { expectedActor1, expectedActor2 };
            CollectionAssert.AreEqual(expectedActors, actualActors);
        }

        [Test]
        public void ShouldAddAndTryRetrieveActor()
        {
            // Given
            var expectedActor = new TestActor();
            manager.AddActor(expectedActor);

            // When
            if (manager.TryGetActor<TestActor>(out var actualActor) == false)
            {
                Assert.Fail("Actor could not be retrieved");
            }

            // Then
            Assert.AreEqual(expectedActor, actualActor);
        }

        [Test]
        public void ShouldAddAndRetrieveActor()
        {
            // Given
            var expectedActor = new TestActor();
            manager.AddActor(expectedActor);

            // When
            var actualActor = manager.GetActor<TestActor>();

            // Then
            Assert.AreEqual(expectedActor, actualActor);
        }

        [Test]
        public void ShouldAddAndRemoveActor()
        {
            // Given
            var actor = new TestActor();
            manager.AddActor(actor);

            // When
            var isRemoved = manager.RemoveActor(actor);

            if (manager.TryGetActor<TestActor>(out _))
            {
                Assert.Fail("Actor was not removed");
            }

            // Then
            var actors = manager.Actors;

            Assert.IsTrue(isRemoved);
            Assert.IsEmpty(actors);
        }

        private sealed class TestActor : IActor
        {
            public bool IsInitialized { get; private set; }

            public void Initialize()
            {
                IsInitialized = true;
            }

            public void Dispose()
            {
                IsInitialized = false;
            }

            public void UpdatePhysics(IUpdateContext context)
            {
            }

            public void UpdateFrame(IUpdateContext context)
            {
            }
        }
    }
}
