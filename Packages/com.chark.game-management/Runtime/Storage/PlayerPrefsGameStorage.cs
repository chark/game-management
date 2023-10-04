using UnityEngine;

namespace CHARK.GameManagement.Storage
{
    /// <summary>
    /// Game storage which stores values in <see cref="PlayerPrefs"/>.
    /// </summary>
    internal sealed class PlayerPrefsGameStorage : GameStorage
    {
        public PlayerPrefsGameStorage(string keyPrefix = "") : base(keyPrefix)
        {
        }

        protected override string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        protected override void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        protected override void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}
