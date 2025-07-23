using System.IO;
using CHARK.GameManagement.Serialization;
using CHARK.GameManagement.Settings;

namespace CHARK.GameManagement.Storage
{
    internal sealed class DefaultFileStorage : Storage
    {
        private readonly string persistentDataPath;
        private readonly IGameManagerSettingsProfile profile;

        public DefaultFileStorage(
            ISerializer serializer,
            IGameManagerSettingsProfile profile,
            string persistentDataPath,
            string pathPrefix = ""
        ) : base(serializer, pathPrefix)
        {
            this.persistentDataPath = persistentDataPath;
            this.profile = profile;
        }

        protected override Stream ReadStream(string path)
        {
            var actualPath = GetFilePath(path);
            if (File.Exists(actualPath) == false)
            {
                if (profile.IsVerboseLogging)
                {
                    GameManager.LogWith(GetType()).LogWarning($"File does not exist: {actualPath}");
                }

                return Stream.Null;
            }

            return File.OpenRead(actualPath);
        }

        protected override void SaveString(string path, string value)
        {
            var actualPath = GetFilePath(path);
            var directory = Path.GetDirectoryName(actualPath);

            if (string.IsNullOrWhiteSpace(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            if (profile.IsVerboseLogging)
            {
                GameManager.LogWith(GetType()).LogInfo($"Saving string to file: {actualPath}");
            }

            File.WriteAllText(actualPath, value);
        }

        protected override void SaveStream(string path, Stream stream)
        {
            var actualPath = GetFilePath(path);
            var directory = Path.GetDirectoryName(actualPath);

            if (string.IsNullOrWhiteSpace(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            if (profile.IsVerboseLogging)
            {
                GameManager
                    .LogWith(GetType())
                    .LogInfo(
                        $"Saving stream (length={stream.Length}, position={stream.Position}) to file: {actualPath}"
                    );
            }

            using var fileStream = new FileStream(actualPath, FileMode.Create);
            stream.CopyTo(fileStream);
        }

        protected override void Delete(string path)
        {
            var actualPath = GetFilePath(path);
            if (File.Exists(actualPath) == false)
            {
                return;
            }

            if (profile.IsVerboseLogging)
            {
                GameManager
                    .LogWith(GetType())
                    .LogInfo($"Deleting file: {actualPath}");
            }

            File.Delete(actualPath);
        }

        private string GetFilePath(string path)
        {
            if (string.IsNullOrWhiteSpace(persistentDataPath))
            {
                return path;
            }

            return Path.Combine(persistentDataPath, path);
        }
    }
}
