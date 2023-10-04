using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHARK.GameManagement.Entities
{
    internal sealed class EntityManager : IEntityManager
    {
        private readonly List<object> entities = new List<object>();
        private readonly bool isVerboseLogging;

        public IReadOnlyList<object> Entities => entities;

        public EntityManager(bool isVerboseLogging = false)
        {
            this.isVerboseLogging = isVerboseLogging;
        }

        public bool AddEntity<TEntity>(TEntity entity) where TEntity : class
        {
#if UNITY_EDITOR
            if (entity == null)
            {
                Debug.LogError("Entity cannot be null");
                return false;
            }
#endif

            if (isVerboseLogging)
            {
                var entityType = entity.GetType();
                var entityName = entityType.Name;

                Debug.Log($"Adding entity {entityName}");
            }

            entities.Add(entity);
            return true;
        }

        public bool RemoveEntity<TEntity>(TEntity entity) where TEntity : class
        {
#if UNITY_EDITOR
            if (entity == null)
            {
                Debug.LogError("Entity cannot be null");
                return false;
            }
#endif

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < entities.Count; index++)
            {
                var otherEntity = entities[index];
                if (otherEntity != entity)
                {
                    continue;
                }

                if (isVerboseLogging)
                {
                    var entityType = entity.GetType();
                    var entityName = entityType.Name;

                    Debug.Log($"Removing entity {entityName}");
                }

                entities.RemoveAt(index);
                return true;
            }

            return false;
        }

        public bool TryGetEntity<TEntity>(out TEntity retrievedEntity)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < entities.Count; index++)
            {
                var entity = entities[index];
                if (!(entity is TEntity typedEntity))
                {
                    continue;
                }

                retrievedEntity = typedEntity;
                return true;
            }

            retrievedEntity = default;
            return false;
        }

        public IEnumerable<TEntity> GetEntities<TEntity>()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < entities.Count; index++)
            {
                var entity = entities[index];
                if (entity is TEntity typedEntity)
                {
                    yield return typedEntity;
                }
            }
        }

        public TEntity GetEntity<TEntity>()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < entities.Count; index++)
            {
                var entity = entities[index];
                if (!(entity is TEntity typedEntity))
                {
                    continue;
                }

                return typedEntity;
            }

            var entityType = typeof(TEntity);
            var entityName = entityType.Name;

            throw new Exception($"Entity {entityName} is not added");
        }
    }
}
