namespace CHARK.GameManagement.Actors
{
    public interface IActorProvider
    {
        /// <returns>
        /// New or recycled instance of an actor.
        /// </returns>
        public IActor Create();
    }
}
