namespace CHARK.GameManagement.Actors
{
    /// <summary>
    /// System which hooks into Unity Lifecycle. Also, useful to avoid having to override
    /// <see cref="ISystem"/> methods.
    /// </summary>
    public abstract class MonoSystem : MonoActor, ISystem
    {
    }
}
