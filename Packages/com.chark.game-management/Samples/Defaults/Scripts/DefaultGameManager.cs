namespace CHARK.GameManagement.Samples.Defaults
{
    internal sealed class DefaultGameManager : GameManager
    {
        protected override void OnInitializeEntered()
        {
            // Initialize systems here
            // AddSystem(new PlayerSystem())
        }

        protected override void OnInitializeExited()
        {
            // Do stuff with systems here after they're initialized
            // var playerSystem = GetSystem<PlayerSystem>();
            // playerSystem.DoStuff();

            // Or initialize some other game related logic
        }

        protected override string GetGameManagerName()
        {
            // Set the name of your manager here. It can be any string.
            return nameof(DefaultGameManager);
        }
    }
}
