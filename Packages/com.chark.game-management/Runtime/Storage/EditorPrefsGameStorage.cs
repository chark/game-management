namespace CHARK.GameManagement.Storage
{
    /// <summary>
    /// Game storage which stores values in <see cref="UnityEditor.EditorPrefs"/>.
    /// </summary>
    internal sealed class EditorPrefsGameStorage : GameStorage
    {
        public EditorPrefsGameStorage(string keyPrefix = "") : base(keyPrefix)
        {
        }

        protected override string GetString(string key)
        {
#if UNITY_EDITOR
            return UnityEditor.EditorPrefs.GetString(key);
#else
            return default;
#endif
        }

        protected override void SetString(string key, string value)
        {
#if UNITY_EDITOR
            UnityEditor.EditorPrefs.SetString(key, value);
#endif
        }

        protected override void DeleteKey(string key)
        {
#if UNITY_EDITOR
            UnityEditor.EditorPrefs.DeleteKey(key);
#endif
        }
    }
}
