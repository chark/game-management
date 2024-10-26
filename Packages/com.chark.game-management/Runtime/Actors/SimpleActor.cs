namespace CHARK.GameManagement.Actors
{
    /// <summary>
    /// Regular system, useful to avoid having to override <see cref="IActor"/> methods.
    /// </summary>
    public abstract class SimpleActor : IActor
    {
        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            OnInitialized();
            IsInitialized = true;
        }

        public void Dispose()
        {
            if (IsInitialized == false)
            {
                return;
            }

            OnDisposed();
            IsInitialized = false;
        }

        public void UpdatePhysics(IUpdateContext context)
        {
            if (IsInitialized == false)
            {
                return;
            }

            OnUpdatedPhysics(context);
        }

        public void UpdateFrame(IUpdateContext context)
        {
            if (IsInitialized == false)
            {
                return;
            }

            OnUpdatedFrame(context);
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual void OnDisposed()
        {
        }

        protected virtual void OnUpdatedPhysics(IUpdateContext context)
        {
        }

        protected virtual void OnUpdatedFrame(IUpdateContext context)
        {
        }
    }
}
