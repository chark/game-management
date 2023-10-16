using CHARK.GameManagement.Serialization;
using CHARK.GameManagement.Storage;
using UnityEditor;

namespace CHARK.GameManagement.Tests.Editor
{
    internal sealed class EditorPrefsStorageTest : StorageTest
    {
        protected override IStorage CreateStorage()
        {
            return new EditorPrefsStorage(DefaultSerializer.Instance);
        }

        protected override string GetString(string key)
        {
            return EditorPrefs.GetString(key);
        }

        protected override void SetString(string key, string value)
        {
            EditorPrefs.SetString(key, value);
        }

        protected override void DeleteString(string key)
        {
            EditorPrefs.DeleteKey(key);
        }
    }
}
