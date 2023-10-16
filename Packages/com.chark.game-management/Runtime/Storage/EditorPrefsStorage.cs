using CHARK.GameManagement.Serialization;

namespace CHARK.GameManagement.Storage
{
    /// <summary>
    /// Game storage which stores values in <see cref="UnityEditor.EditorPrefs"/>.
    /// </summary>
    internal sealed class EditorPrefsStorage : Storage
    {
        public EditorPrefsStorage(ISerializer serializer, string keyPrefix = "") : base(serializer, keyPrefix)
        {
        }

        protected override string GetString(string path)
        {
#if UNITY_EDITOR
            return UnityEditor.EditorPrefs.GetString(path);
#else
            return default;
#endif
        }

        protected override void SetString(string path, string value)
        {
#if UNITY_EDITOR
            UnityEditor.EditorPrefs.SetString(path, value);
#endif
        }

        protected override void Delete(string path)
        {
#if UNITY_EDITOR
            UnityEditor.EditorPrefs.DeleteKey(path);
#endif
        }
    }
}
