using System;
using System.Collections.Generic;
using CHARK.GameManagement.Settings;
using CHARK.GameManagement.Utilities;

namespace CHARK.GameManagement.Entities
{
    internal sealed class DefaultEntityManager : IEntityManager
    {
        private readonly List<object> entities = new();
        private readonly IGameManagerSettingsProfile profile;

        public IReadOnlyList<object> Entities => entities;

        public DefaultEntityManager(IGameManagerSettingsProfile profile)
        {
            this.profile = profile;
        }

        public bool AddEntity<TEntity>(TEntity entity) where TEntity : class
        {
#if UNITY_EDITOR
            if (entity == null)
            {
                Logging.LogError("Entity cannot be null", GetType());
                return false;
            }
#endif

            if (profile.IsVerboseLogging)
            {
                var entityType = entity.GetType();
                var entityName = entityType.Name;

                Logging.LogDebug($"Adding entity {entityName}", GetType());
            }

            entities.Add(entity);
            return true;
        }

        public bool RemoveEntity<TEntity>(TEntity entity) where TEntity : class
        {
#if UNITY_EDITOR
            if (entity == null)
            {
                Logging.LogError("Entity cannot be null", GetType());
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

                if (profile.IsVerboseLogging)
                {
                    var entityType = entity.GetType();
                    var entityName = entityType.Name;

                    Logging.LogDebug($"Removing entity {entityName}", GetType());
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
