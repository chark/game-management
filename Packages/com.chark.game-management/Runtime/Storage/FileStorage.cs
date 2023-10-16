using System.IO;
using CHARK.GameManagement.Serialization;

namespace CHARK.GameManagement.Storage
{
    internal sealed class FileStorage : Storage
    {
        private readonly string persistentDataPath;

        public FileStorage(ISerializer serializer, string persistentDataPath, string pathPrefix = "") : base(serializer, pathPrefix)
        {
            this.persistentDataPath = persistentDataPath;
        }

        protected override string GetString(string path)
        {
            var actualPath = GetFilePath(path);
            if (File.Exists(actualPath) == false)
            {
                return default;
            }

            return File.ReadAllText(actualPath);
        }

        protected override void SetString(string path, string value)
        {
            var actualPath = GetFilePath(path);
            File.WriteAllText(actualPath, value);
        }

        protected override void Delete(string path)
        {
            var actualPath = GetFilePath(path);
            if (File.Exists(actualPath) == false)
            {
                return;
            }

            File.Delete(actualPath);
        }

        private string GetFilePath(string path)
        {
            var fileName = $"{path}.json";
            var actualPath = Path.Combine(persistentDataPath, fileName);

            return actualPath;
        }
    }
}
