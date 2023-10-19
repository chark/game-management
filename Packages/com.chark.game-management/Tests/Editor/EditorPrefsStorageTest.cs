using CHARK.GameManagement.Serialization;
using CHARK.GameManagement.Storage;
using UnityEditor;

namespace CHARK.GameManagement.Tests.Editor
{
    // ReSharper disable once UnusedType.Global
    internal sealed class EditorPrefsStorageTest : StorageTest
    {
        protected override IStorage CreateStorage()
        {
            return new EditorPrefsStorage(DefaultSerializer.Instance);
        }

        protected override string ReadString(string path)
        {
            return EditorPrefs.GetString(path);
        }

        protected override void SaveString(string path, string data)
        {
            EditorPrefs.SetString(path, data);
        }

        protected override void DeleteString(string path)
        {
            EditorPrefs.DeleteKey(path);
        }
    }
}
