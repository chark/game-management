using System.IO;
using CHARK.GameManagement.Serialization;
using CHARK.GameManagement.Storage;
using UnityEngine;

namespace CHARK.GameManagement.Tests.Editor
{
    // ReSharper disable once UnusedType.Global
    internal sealed class DefaultFileStorageTest : StorageTest
    {
        protected override IStorage CreateStorage()
        {
            return new DefaultFileStorage(
                serializer: DefaultSerializer.Instance,
                profile: GameManagerTestProfile.Instance,
                persistentDataPath: Application.persistentDataPath,
                pathPrefix: "Tests/"
            );
        }

        protected override string ReadString(string path)
        {
            var actualPath = GetTestPath(path);
            return File.ReadAllText(actualPath);
        }

        protected override void SaveString(string path, string data)
        {
            var actualPath = GetTestPath(path);
            var directory = Path.GetDirectoryName(actualPath);

            if (string.IsNullOrWhiteSpace(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(actualPath, data);
        }

        protected override void DeleteString(string path)
        {
            var actualPath = GetTestPath(path);
            File.Delete(actualPath);
        }

        private static string GetTestPath(string path)
        {
            return Path.Combine(Application.persistentDataPath, "Tests", path);
        }
    }
}
