namespace CHARK.GameManagement.Settings
{
    internal enum InstantiationMode
    {
        /// <summary>
        /// <see cref="GameManager"/> is instantiated before the scene loads.
        /// </summary>
        BeforeSceneLoad,

        /// <summary>
        /// <see cref="GameManager"/> is instantiated after the scene loads.
        /// </summary>
        AfterSceneLoad,
    }
}
