using System.IO;

namespace CHARK.GameManagement.Storage
{
    internal sealed class FileGameStorage : GameStorage
    {
        private readonly string persistentDataPath;

        public FileGameStorage(string persistentDataPath, string keyPrefix = "") : base(keyPrefix)
        {
            this.persistentDataPath = persistentDataPath;
        }

        protected override string GetString(string key)
        {
            var path = GetFilePath(key);
            if (File.Exists(path) == false)
            {
                return default;
            }

            return File.ReadAllText(path);
        }

        protected override void SetString(string key, string value)
        {
            var path = GetFilePath(key);
            File.WriteAllText(path, value);
        }

        protected override void DeleteKey(string key)
        {
            var path = GetFilePath(key);
            if (File.Exists(path) == false)
            {
                return;
            }

            File.Delete(path);
        }

        private string GetFilePath(string key)
        {
            var fileName = $"{key}.json";
            var path = Path.Combine(persistentDataPath, fileName);

            return path;
        }
    }
}
