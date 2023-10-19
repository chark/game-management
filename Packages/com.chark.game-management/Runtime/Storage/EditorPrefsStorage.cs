using System.IO;
using System.Text;
using CHARK.GameManagement.Serialization;

namespace CHARK.GameManagement.Storage
{
    /// <summary>
    /// Game storage which stores values in <see cref="UnityEditor.EditorPrefs"/>.
    /// </summary>
    internal sealed class EditorPrefsStorage : Storage
    {
        public EditorPrefsStorage(ISerializer serializer, string pathPrefix = "") : base(serializer, pathPrefix)
        {
        }

        protected override Stream ReadStream(string path)
        {
#if UNITY_EDITOR
            var editorString = UnityEditor.EditorPrefs.GetString(path);
            if (string.IsNullOrWhiteSpace(editorString))
            {
                return Stream.Null;
            }

            var bytes = Encoding.UTF8.GetBytes(editorString);
            return new MemoryStream(bytes);
#else
            return Stream.Null;
#endif
        }

        protected override void SaveString(string path, string value)
        {
#if UNITY_EDITOR
            UnityEditor.EditorPrefs.SetString(path, value);
#endif
        }

        protected override void SaveStream(string path, Stream stream)
        {
#if UNITY_EDITOR
            using var streamReader = new StreamReader(stream);
            var value = streamReader.ReadToEnd();

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
