using System.Collections.Generic;

namespace CHARK.GameManagement.Entities
{
    public interface IEntityManager
    {
        /// <summary>
        /// List of all entities added to the entity manager.
        /// </summary>
        public IReadOnlyList<object> Entities { get; }

        /// <summary>
        /// Add a <paramref name="entity"/> to the Entity Manager.
        /// </summary>
        /// <returns>
        /// <c>true</c> if given <paramref name="entity"/> was added or <c>false</c> otherwise.
        /// </returns>
        public bool AddEntity<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Remove a <paramref name="entity"/> from the Entity Manager.
        /// </summary>
        /// <returns>
        /// <c>true</c> if given <paramref name="entity"/> was removed or <c>false</c> otherwise.
        /// </returns>
        public bool RemoveEntity<TEntity>(TEntity entity) where TEntity : class;

        /// <returns>
        /// <c>true</c> if <paramref name="entity"/> is retrieved or <c>false</c> otherwise.
        /// </returns>
        public bool TryGetEntity<TEntity>(out TEntity entity);

        /// <returns>
        /// Enumerable of added entities of type <see cref="TEntity"/>.
        /// </returns>
        public IEnumerable<TEntity> GetEntities<TEntity>();

        /// <returns>
        /// Entity of type <see cref="TEntity"/> or throws <see cref="System.Exception"/> if entity
        /// is not added to the manager.
        /// </returns>
        public TEntity GetEntity<TEntity>();
    }
}
