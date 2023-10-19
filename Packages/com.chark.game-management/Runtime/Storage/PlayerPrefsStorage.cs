using System.IO;
using System.Text;
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

        protected override Stream ReadStream(string path)
        {
            var prefsString = PlayerPrefs.GetString(path);
            if (string.IsNullOrWhiteSpace(prefsString))
            {
                return Stream.Null;
            }

            var prefsBytes = Encoding.UTF8.GetBytes(prefsString);

            return new MemoryStream(prefsBytes);
        }

        protected override void SaveString(string path, string value)
        {
            PlayerPrefs.SetString(path, value);
        }

        protected override void SaveStream(string path, Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            var value = streamReader.ReadToEnd();

            PlayerPrefs.SetString(path, value);
        }

        protected override void Delete(string path)
        {
            PlayerPrefs.DeleteKey(path);
        }
    }
}
