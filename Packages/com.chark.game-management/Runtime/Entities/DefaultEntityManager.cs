using System;
using System.Collections.Generic;
using System.Linq;
using CHARK.GameManagement.Settings;

namespace CHARK.GameManagement.Entities
{
    internal sealed class DefaultEntityManager : IEntityManager
    {
        private readonly IDictionary<Type, List<object>> entityLookupCache = new Dictionary<Type, List<object>>();
        private readonly List<object> entities = new();

        private readonly IGameManagerSettingsProfile profile;

        public int CachedEntityCount => entityLookupCache.Sum(pair => pair.Value.Count);

        public int CachedTypeCount => entityLookupCache.Keys.Count;

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
                GameManager.LogWith(GetType()).LogError("Entity cannot be null");
                return false;
            }
#endif

            if (profile.IsVerboseLogging)
            {
                var entityType = entity.GetType();
                var entityName = entityType.Name;

                GameManager.LogWith(GetType()).LogInfo($"Adding entity {entityName}");
            }

            entities.Add(entity);

            var cache = GetEntityCache<TEntity>();
            cache.Add(entity);

            return true;
        }

        public bool RemoveEntity<TEntity>(TEntity entity) where TEntity : class
        {
#if UNITY_EDITOR
            if (entity == null)
            {
                GameManager.LogWith(GetType()).LogError("Entity cannot be null");
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

                    GameManager.LogWith(GetType()).LogInfo($"Removing entity {entityName}");
                }

                entities.RemoveAt(index);

                if (entityLookupCache.TryGetValue(typeof(TEntity), out var cache))
                {
                    cache.Remove(entity);
                    if (cache.Count <= 0)
                    {
                        entityLookupCache.Remove(typeof(TEntity));
                    }
                }

                return true;
            }

            return false;
        }

        public bool TryGetEntity<TEntity>(out TEntity retrievedEntity)
        {
            if (TryGetCachedEntity<TEntity>(out var cachedEntity))
            {
                retrievedEntity = cachedEntity;
                return true;
            }

            var cache = GetEntityCache<TEntity>();

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < entities.Count; index++)
            {
                var entity = entities[index];
                if (entity is not TEntity typedEntity)
                {
                    continue;
                }

                cache.Add(entity);

                retrievedEntity = typedEntity;
                return true;
            }

            retrievedEntity = default;
            return false;
        }

        public IEnumerable<TEntity> GetEntities<TEntity>()
        {
            var cache = GetEntityCache<TEntity>();
            if (cache.Count > 0)
            {
                for (var index = 0; index < cache.Count; index++)
                {
                    var entity = cache[index];
                    yield return (TEntity)entity;
                }

                yield break;
            }

            for (var index = 0; index < entities.Count; index++)
            {
                var entity = entities[index];
                if (entity is not TEntity typedEntity)
                {
                    continue;
                }

                cache.Add(entity);

                yield return typedEntity;
            }
        }

        public TEntity GetEntity<TEntity>()
        {
            if (TryGetCachedEntity<TEntity>(out var cachedEntity))
            {
                return cachedEntity;
            }

            var cache = GetEntityCache<TEntity>();

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < entities.Count; index++)
            {
                var entity = entities[index];
                if (entity is not TEntity typedEntity)
                {
                    continue;
                }

                cache.Add(entity);

                return typedEntity;
            }

            var entityType = typeof(TEntity);
            var entityName = entityType.Name;

            throw new Exception($"Entity {entityName} is not added");
        }

        private bool TryGetCachedEntity<TEntity>(out TEntity retrievedEntity)
        {
            if (entityLookupCache.TryGetValue(typeof(TEntity), out var cache) && cache.Count > 0)
            {
                retrievedEntity = (TEntity)cache[0];
                return true;
            }

            retrievedEntity = default;
            return false;
        }

        private IList<object> GetEntityCache<TEntity>()
        {
            if (entityLookupCache.TryGetValue(typeof(TEntity), out var existingCache))
            {
                return existingCache;
            }

            var newCache = new List<object>();
            entityLookupCache[typeof(TEntity)] = newCache;
            return newCache;
        }
    }
}
