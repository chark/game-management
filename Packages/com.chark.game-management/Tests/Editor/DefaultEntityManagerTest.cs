using CHARK.GameManagement.Entities;
using CHARK.GameManagement.Settings;
using NUnit.Framework;

namespace CHARK.GameManagement.Tests.Editor
{
    internal sealed class DefaultEntityManagerTest
    {
        private DefaultEntityManager entityManager;

        [SetUp]
        public void SetUp()
        {
            entityManager = new DefaultEntityManager(DefaultGameManagerSettingsProfile.Instance);
        }

        [Test]
        public void ShouldAddAndRetrieveEntity()
        {
            // Given
            var expectedEntity = new TestPlayerEntity();

            // When
            entityManager.AddEntity(expectedEntity);
            var actualEntity = entityManager.GetEntity<TestPlayerEntity>();

            // Then
            Assert.AreEqual(expectedEntity, actualEntity);

            Assert.AreEqual(1, entityManager.CachedEntityCount);
            Assert.AreEqual(1, entityManager.CachedTypeCount);
        }

        [Test]
        public void ShouldAddTwoEntitiesAndRetrieveFirstEntity()
        {
            // Given
            var expectedEntity = new TestPlayerEntity();

            // When
            entityManager.AddEntity(expectedEntity);
            entityManager.AddEntity(new TestPlayerEntity());

            var actualEntity = entityManager.GetEntity<TestPlayerEntity>();

            // Then
            Assert.AreEqual(expectedEntity, actualEntity);

            Assert.AreEqual(2, entityManager.CachedEntityCount);
            Assert.AreEqual(1, entityManager.CachedTypeCount);
        }

        [Test]
        public void ShouldAddAndRetrieveEntities()
        {
            // Given
            var expectedEntityA = new TestPlayerEntity();
            var expectedEntityB = new TestPlayerEntity();
            var expectedEntityC = new TestEnemyEntity();

            // When
            entityManager.AddEntity(expectedEntityA);
            entityManager.AddEntity(expectedEntityB);
            entityManager.AddEntity(expectedEntityC);

            var testPlayers = entityManager.GetEntities<TestPlayerEntity>();
            var testEnemies = entityManager.GetEntities<TestEnemyEntity>();

            // Then
            CollectionAssert.AreEquivalent(new[] { expectedEntityA, expectedEntityB }, testPlayers);
            CollectionAssert.AreEquivalent(new[] { expectedEntityC }, testEnemies);

            Assert.AreEqual(3, entityManager.CachedEntityCount);
            Assert.AreEqual(2, entityManager.CachedTypeCount);
        }

        [Test]
        public void ShouldAddAndRemoveEntity()
        {
            // Given
            var entity = new TestPlayerEntity();

            // When
            entityManager.AddEntity(entity);
            entityManager.RemoveEntity(entity);

            // Then
            CollectionAssert.IsEmpty(entityManager.Entities);

            Assert.Zero(entityManager.CachedEntityCount);
            Assert.Zero(entityManager.CachedTypeCount);
        }

        [Test]
        public void ShouldAddAndRemoveEntities()
        {
            // Given
            var entityA = new TestPlayerEntity();
            var entityB = new TestPlayerEntity();
            var entityC = new TestEnemyEntity();

            // When
            entityManager.AddEntity(entityA);
            entityManager.AddEntity(entityB);
            entityManager.AddEntity(entityC);

            entityManager.RemoveEntity(entityA);
            entityManager.RemoveEntity(entityB);
            entityManager.RemoveEntity(entityC);

            // Then
            CollectionAssert.IsEmpty(entityManager.Entities);

            Assert.Zero(entityManager.CachedEntityCount);
            Assert.Zero(entityManager.CachedTypeCount);
        }

        private class TestPlayerEntity
        {
        }

        private class TestEnemyEntity
        {
        }
    }
}
