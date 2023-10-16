using CHARK.GameManagement.Serialization;
using UnityEngine;

namespace CHARK.GameManagement.Storage
{
    /// <summary>
    /// Game storage which stores values in <see cref="PlayerPrefs"/>.
    /// </summary>
    internal sealed class PlayerPrefsStorage : Storage
    {
        public PlayerPrefsStorage(ISerializer serializer, string pathPrefix = "") : base(serializer, pathPrefix)
        {
        }

        protected override string GetString(string path)
        {
            return PlayerPrefs.GetString(path);
        }

        protected override void SetString(string path, string value)
        {
            PlayerPrefs.SetString(path, value);
        }

        protected override void Delete(string path)
        {
            PlayerPrefs.DeleteKey(path);
        }
    }
}
