using System;
using System.Collections.Generic;
using CHARK.GameManagement.Settings;
using CHARK.GameManagement.Utilities;

namespace CHARK.GameManagement.Actors
{
    internal sealed class DefaultActorManager : IActorManager
    {
        // TODO: optimized, use dict by type?
        private readonly HashSet<IActor> actorLookupCache = new();
        private readonly List<IActor> actors = new();

        // TODO: do something with providers
        private readonly HashSet<IActorProvider> actorProviderLookupCache = new();
        private readonly List<IActorProvider> actorProviders = new();

        private readonly IGameManagerSettingsProfile profile;

        public IReadOnlyList<IActor> Actors => actors;

        public DefaultActorManager(IGameManagerSettingsProfile profile)
        {
            this.profile = profile;
        }

        public bool AddProvider(IActorProvider provider)
        {
#if UNITY_EDITOR
            if (provider == null)
            {
                Logging.LogError($"{nameof(provider)} cannot be null", GetType());
                return false;
            }
#endif

            if (actorProviderLookupCache.Contains(provider))
            {
                return false;
            }

            actorProviders.Add(provider);
            actorProviderLookupCache.Add(provider);

            return true;
        }

        public bool RemoveProvider(IActorProvider provider)
        {
#if UNITY_EDITOR
            if (provider == null)
            {
                Logging.LogError($"{nameof(provider)} cannot be null", GetType());
                return false;
            }
#endif

            if (actorProviders.Remove(provider) == false)
            {
                return false;
            }

            actorProviders.Remove(provider);
            actorProviderLookupCache.Remove(provider);

            return true;
        }

        public bool AddActor<TActor>(TActor actor) where TActor : IActor
        {
#if UNITY_EDITOR
            if (actor == null)
            {
                Logging.LogError($"{nameof(actor)} cannot be null", GetType());
                return false;
            }
#endif

            if (actorLookupCache.Contains(actor))
            {
                if (profile.IsVerboseLogging)
                {
                    var actorType = actor.GetType();
                    var actorName = actorType.Name;

                    Logging.LogDebug($"Actor {actorName} is already added", GetType());
                }

                return false;
            }

            actors.Add(actor);
            actorLookupCache.Add(actor);

            if (profile.IsVerboseLogging)
            {
                var actorType = actor.GetType();
                var actorName = actorType.Name;

                Logging.LogDebug($"Added actor {actorName}", GetType());
            }

            return true;
        }

        public bool RemoveActor<TActor>(TActor actor) where TActor : IActor
        {
#if UNITY_EDITOR
            if (actor == null)
            {
                Logging.LogError($"{nameof(actor)} cannot be null", GetType());
                return false;
            }
#endif

            // ReSharper disable once ForCanBeConvertedToForeach
            if (actors.Remove(actor) == false)
            {
                return false;
            }

            actorLookupCache.Remove(actor);

            if (profile.IsVerboseLogging)
            {
                var actorType = actor.GetType();
                var actorName = actorType.Name;

                Logging.LogDebug($"Removed actor {actorName}", GetType());
            }

            return true;
        }

        public bool TryGetActor<TActor>(out TActor actor) where TActor : IActor
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < actors.Count; index++)
            {
                var otherActor = actors[index];
                if (otherActor is not TActor typedActor)
                {
                    continue;
                }

                actor = typedActor;
                return true;
            }

            actor = default;
            return false;
        }

        // TODO: we allocate enumerable, can we reduce GC work somehow?
        public IEnumerable<TActor> GetActors<TActor>() where TActor : IActor
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < actors.Count; index++)
            {
                var otherActor = actors[index];
                if (otherActor is TActor typedActor)
                {
                    yield return typedActor;
                }
            }
        }

        // TODO: can we speedup lookup by type somehow?
        public TActor GetActor<TActor>() where TActor : IActor
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < actors.Count; index++)
            {
                var otherActor = actors[index];
                if (otherActor is TActor typedActor)
                {
                    return typedActor;
                }
            }

            var actorType = typeof(TActor);
            var actorName = actorType.Name;

            throw new Exception($"Actor {actorName} is not added to the manager");
        }
    }
}
