using CHARK.GameManagement.Serialization;
using CHARK.GameManagement.Storage;
using UnityEngine;

namespace CHARK.GameManagement.Tests.Editor
{
    internal sealed class PlayerPrefsStorageTest : StorageTest
    {
        protected override IStorage CreateStorage()
        {
            return new PlayerPrefsStorage(DefaultSerializer.Instance);
        }

        protected override string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        protected override void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        protected override void DeleteString(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}
