using CHARK.GameManagement.Storage;
using UnityEngine;

namespace CHARK.GameManagement.Tests.Editor
{
    internal sealed class PlayerPrefsGameStorageTest : GameStorageTest
    {
        protected override IGameStorage CreateStorage()
        {
            return new PlayerPrefsGameStorage();
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
