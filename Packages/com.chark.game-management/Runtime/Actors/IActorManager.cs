using System;
using System.Collections.Generic;

namespace CHARK.GameManagement.Actors
{
    public interface IActorManager
    {
        /// <summary>
        /// Collection of all actors added to this manager.
        /// </summary>
        public IReadOnlyList<IActor> Actors { get; }

        /// <summary>
        /// Add given <paramref name="provider"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if given <paramref name="provider"/> was added or <c>false</c> otherwise.
        /// </returns>
        public bool AddProvider(IActorProvider provider);

        /// <summary>
        /// Remove given <paramref name="provider"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if given <paramref name="provider"/> was removed or <c>false</c> otherwise.
        /// </returns>
        public bool RemoveProvider(IActorProvider provider);

        /// <summary>
        /// Add an <paramref name="actor"/> of given type to this manager.
        /// </summary>
        /// <returns>
        /// <c>true</c> if given <paramref name="actor"/> was added or <c>false</c> otherwise.
        /// </returns>
        public bool AddActor<TActor>(TActor actor) where TActor : IActor;

        /// <summary>
        /// Remove an <paramref name="actor"/> if type <see cref="TActor"/> from this manager.
        /// </summary>
        /// <returns>
        /// <c>true</c> if given <paramref name="actor"/> was removed or <c>false</c> otherwise.
        /// </returns>
        public bool RemoveActor<TActor>(TActor actor) where TActor : IActor;

        /// <returns>
        /// <c>true</c> if <paramref name="actor"/> of the type <see cref="TActor"/> is retrieved or
        /// <c>false</c> otherwise.
        /// </returns>
        public bool TryGetActor<TActor>(out TActor actor) where TActor : IActor;

        /// <returns>
        /// Enumerable containing added actors of the given type.
        /// </returns>
        public IEnumerable<TActor> GetActors<TActor>() where TActor : IActor;

        /// <returns>
        /// Get the first actor of type <see cref="TActor"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// if the actor of given type is not found.
        /// </exception>
        public TActor GetActor<TActor>() where TActor : IActor;
    }
}
