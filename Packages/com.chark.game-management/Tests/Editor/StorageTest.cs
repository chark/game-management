using System.Globalization;
using System.IO;
using System.Text;
using CHARK.GameManagement.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace CHARK.GameManagement.Tests.Editor
{
    internal abstract class StorageTest
    {
        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        private string Path => GetType().Name;

        private IStorage storage;

        [SetUp]
        public void SetUp()
        {
            storage = CreateStorage();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteString(Path);
        }

        [Test]
        public void ShouldReadPrimitiveData()
        {
            // Given:
            // - Persisted primitive float data of 1.
            const float expectedData = 1f;
            SaveString(Path, expectedData.ToString(CultureInfo.InvariantCulture));

            // When:
            // - Retrieving data from storage.
            if (storage.TryReadData(Path, out float actualData) == false)
            {
                Assert.Fail($"Could not {nameof(storage.TryReadData)} from primitive");
            }

            // Then:
            // - Data should be retrieved properly.
            Assert.AreEqual(expectedData, actualData);
        }

        [Test]
        public void ShouldReadStructData()
        {
            // Given:
            // - Persisted struct data.
            var expectedData = new TestStructData("hello");
            SaveString(Path, SerializeObject(expectedData));

            // When:
            // - Retrieving data from storage.
            if (storage.TryReadData(Path, out TestStructData actualData) == false)
            {
                Assert.Fail($"Could not {nameof(storage.TryReadData)} from struct");
            }

            // Then:
            // - Data (contents) should be retrieved properly.
            Assert.AreEqual(expectedData.Text, actualData.Text);
        }

        [Test]
        public void ShouldReadObjectData()
        {
            // Given:
            // - Persisted object data.
            var expectedData = new TestObjectData("hello");
            SaveString(Path, SerializeObject(expectedData));

            // When:
            // - Retrieving data from storage.
            if (storage.TryReadData(Path, out TestObjectData actualData) == false)
            {
                Assert.Fail($"Could not {nameof(storage.TryReadData)} from object");
            }

            // Then:
            // - Data (contents) should be retrieved properly.
            Assert.AreEqual(expectedData.Text, actualData.Text);
        }

        [Test]
        public void ShouldReadNoDataOnMissingData()
        {
            // Given:
            // - No persisted data.
            DeleteString(Path);

            // When:
            // - Retrieving data from storage.
            var isRead = storage.TryReadData<string>(Path, out var data);

            // Then:
            // - Data should not be retrieved.
            Assert.IsFalse(isRead);
            Assert.IsNull(data);
        }

        [Test]
        public void ShouldReadStreamObjectData()
        {
            // Given:
            // - Persisted object data.
            var expectedData = new TestObjectData("hello");
            SaveString(Path, SerializeObject(expectedData));

            // When:
            // - Retrieving data from storage.
            var actualStream = storage.ReadDataStream(Path);
            if (actualStream.Length == 0)
            {
                Assert.Fail($"Could not {nameof(storage.ReadDataStream)} from object");
            }

            // Then:
            // - Data (contents) should be retrieved properly.
            var actualData = DeserializeObject<TestObjectData>(actualStream);

            Assert.AreEqual(expectedData.Text, actualData.Text);
        }

        [Test]
        public void ShouldReadEmptyStreamOnMissingData()
        {
            // Given:
            // - No persisted data.
            DeleteString(Path);

            // When:
            // - Retrieving data from storage.
            var stream = storage.ReadDataStream(Path);

            // Then:
            // - Data (contents) should be retrieved properly.
            Assert.AreEqual(0, stream.Length);
        }

        [Test]
        public void ShouldSavePrimitiveData()
        {
            // Given:
            // - Primitive float data of 1.
            const float expectedData = 1f;

            // When:
            // - Persisting data.
            storage.SaveData(Path, expectedData);

            // Then:
            // - Data should be persisted properly.
            var actualData = float.Parse(ReadString(Path));
            Assert.AreEqual(expectedData, actualData);
        }

        [Test]
        public void ShouldSaveStructData()
        {
            // Given:
            // - Struct data.
            var data = new TestStructData("hello there");

            // When:
            // - Persisting data.
            storage.SaveData(Path, data);

            // Then:
            // - Data should be persisted properly.
            var expectedData = SerializeObject(data);
            var actualData = ReadString(Path);

            Assert.AreEqual(expectedData, actualData);
        }

        [Test]
        public void ShouldSaveObjectData()
        {
            // Given:
            // - Data object.
            var data = new TestObjectData("hello there");

            // When:
            // - Persisting data.
            storage.SaveData(Path, data);

            // Then:
            // - Data should be persisted properly.
            var expectedData = SerializeObject(data);
            var actualData = ReadString(Path);

            Assert.AreEqual(expectedData, actualData);
        }

        [Test]
        public void ShouldSaveStreamObjectData()
        {
            // Given:
            // - Object data.
            var data = new TestObjectData("hello there");
            var expectedData = SerializeObject(data);

            // When:
            // - Persisting data.
            using var stream = new MemoryStream(
                Encoding.UTF8.GetBytes(expectedData)
            );

            storage.SaveDataStream(Path, stream);

            // Then:
            // - Data should be persisted properly.
            var actualData = ReadString(Path);

            Assert.AreEqual(expectedData, actualData);
        }

        /// <returns>
        /// New storage instance.
        /// </returns>
        protected abstract IStorage CreateStorage();

        /// <returns>
        /// Raw json data retrieved by given <paramref name="path"/>.
        /// </returns>
        protected abstract string ReadString(string path);

        /// <summary>
        /// Set raw json data retrieved at given <paramref name="path"/>.
        /// </summary>
        protected abstract void SaveString(string path, string data);

        /// <summary>
        /// Delete raw json stored at given <paramref name="path"/>.
        /// </summary>
        protected abstract void DeleteString(string path);

        private static TObject DeserializeObject<TObject>(Stream stream)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8);
            var @object = reader.ReadToEnd();

            return JsonConvert.DeserializeObject<TObject>(@object, SerializerSettings);
        }

        private static string SerializeObject<TObject>(TObject @object)
        {
            return JsonConvert.SerializeObject(@object, SerializerSettings);
        }

        private class TestObjectData
        {
            public string Text { get; }

            [JsonConstructor]
            public TestObjectData(string text)
            {
                Text = text;
            }
        }

        private readonly struct TestStructData
        {
            public string Text { get; }

            [JsonConstructor]
            public TestStructData(string text)
            {
                Text = text;
            }
        }
    }
}
