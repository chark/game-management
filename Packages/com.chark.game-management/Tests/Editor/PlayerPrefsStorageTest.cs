using CHARK.GameManagement.Serialization;
using CHARK.GameManagement.Storage;
using UnityEngine;

namespace CHARK.GameManagement.Tests.Editor
{
    // ReSharper disable once UnusedType.Global
    internal sealed class PlayerPrefsStorageTest : StorageTest
    {
        protected override IStorage CreateStorage()
        {
            return new PlayerPrefsStorage(DefaultSerializer.Instance);
        }

        protected override string ReadString(string path)
        {
            return PlayerPrefs.GetString(path);
        }

        protected override void SaveString(string path, string data)
        {
            PlayerPrefs.SetString(path, data);
        }

        protected override void DeleteString(string path)
        {
            PlayerPrefs.DeleteKey(path);
        }
    }
}
