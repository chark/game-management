using System;
using System.Collections.Generic;
using CHARK.GameManagement.Settings;
using CHARK.GameManagement.Utilities;

namespace CHARK.GameManagement.Entities
{
    internal sealed class DefaultEntityManager : IEntityManager
    {
        private readonly IDictionary<Type, object> entityLookupCache = new Dictionary<Type, object>();
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

            entityLookupCache[typeof(TEntity)] = entity;
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

                entityLookupCache.Remove(typeof(TEntity));
                entities.RemoveAt(index);
                return true;
            }

            return false;
        }

        public bool TryGetEntity<TEntity>(out TEntity retrievedEntity)
        {
            if (entityLookupCache.TryGetValue(typeof(TEntity), out var cachedEntity))
            {
                retrievedEntity = (TEntity)cachedEntity;
                return true;
            }

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < entities.Count; index++)
            {
                var entity = entities[index];
                if (!(entity is TEntity typedEntity))
                {
                    continue;
                }

                entityLookupCache[typeof(TEntity)] = typedEntity;
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
            if (entityLookupCache.TryGetValue(typeof(TEntity), out var cachedEntity))
            {
                return (TEntity)cachedEntity;
            }

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < entities.Count; index++)
            {
                var entity = entities[index];
                if (!(entity is TEntity typedEntity))
                {
                    continue;
                }

                entityLookupCache[typeof(TEntity)] = typedEntity;
                return typedEntity;
            }

            var entityType = typeof(TEntity);
            var entityName = entityType.Name;

            throw new Exception($"Entity {entityName} is not added");
        }
    }
}
