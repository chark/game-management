using System.IO;
using CHARK.GameManagement.Serialization;
using CHARK.GameManagement.Settings;
using CHARK.GameManagement.Utilities;

namespace CHARK.GameManagement.Storage
{
    internal sealed class FileStorage : Storage
    {
        private readonly string persistentDataPath;
        private readonly IGameManagerSettingsProfile profile;

        public FileStorage(
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
                    Logging.LogWarning($"File does not exist: {actualPath}", GetType());
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
                Logging.LogDebug($"Saving string to file: {actualPath}", GetType());
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
                Logging.LogDebug(
                    $"Saving stream (length={stream.Length}, position={stream.Position}) to file: {actualPath}",
                    GetType()
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
                Logging.LogDebug($"Deleting file: {actualPath}", GetType());
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
