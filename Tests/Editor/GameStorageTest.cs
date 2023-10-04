using System.Globalization;
using CHARK.GameManagement.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace CHARK.GameManagement.Tests.Editor
{
    internal abstract class GameStorageTest
    {
        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        private string Key => GetType().Name;

        private IGameStorage storage;

        [SetUp]
        public void SetUp()
        {
            storage = CreateStorage();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteString(Key);
        }

        [Test]
        public void ShouldGetPrimitiveValue()
        {
            // Given:
            // - Persisted primitive float value of 1.
            const float expectedValue = 1f;
            SetString(Key, expectedValue.ToString(CultureInfo.InvariantCulture));

            // When:
            // - Retrieving value from storage.
            if (storage.TryGetValue(Key, out float actualValue) == false)
            {
                Assert.Fail("Could not get Runtime primitive value");
            }

            // Then:
            // - Value should be retrieved properly.
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void ShouldGetStructValue()
        {
            // Given:
            // - Persisted struct value.
            var expectedValue = new TestStructData("hello");
            SetString(Key, SerializeObject(expectedValue));

            // When:
            // - Retrieving value from storage.
            if (storage.TryGetValue(Key, out TestStructData actualValue) == false)
            {
                Assert.Fail("Could not get Runtime struct value");
            }

            // Then:
            // - Value (contents) should be retrieved properly.
            Assert.AreEqual(expectedValue.Text, actualValue.Text);
        }

        [Test]
        public void ShouldGetObjectValue()
        {
            // Given:
            // - Persisted object value.
            var expectedValue = new TestObjectData("hello");
            SetString(Key, SerializeObject(expectedValue));

            // When:
            // - Retrieving value from storage.
            if (storage.TryGetValue(Key, out TestObjectData actualValue) == false)
            {
                Assert.Fail("Could not get Runtime object value");
            }

            // Then:
            // - Value (contents) should be retrieved properly.
            Assert.AreEqual(expectedValue.Text, actualValue.Text);
        }

        [Test]
        public void ShouldSetPrimitiveValue()
        {
            // Given:
            // - Primitive float value of 1.
            const float expectedValue = 1f;

            // When:
            // - Persisting value.
            storage.SetValue(Key, expectedValue);

            // Then:
            // - Value should be persisted properly.
            var actualValue = float.Parse(GetString(Key));
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void ShouldSetStructValue()
        {
            // Given:
            // - Struct value.
            var value = new TestStructData("hello there");

            // When:
            // - Persisting value.
            storage.SetValue(Key, value);

            // Then:
            // - Value should be persisted properly.
            var expectedValue = SerializeObject(value);
            var actualValue = GetString(Key);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void ShouldSetObjectValue()
        {
            // Given:
            // - Object value.
            var value = new TestObjectData("hello there");

            // When:
            // - Persisting value.
            storage.SetValue(Key, value);

            // Then:
            // - Value should be persisted properly.
            var expectedValue = SerializeObject(value);
            var actualValue = GetString(Key);

            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <returns>
        /// New storage instance.
        /// </returns>
        protected abstract IGameStorage CreateStorage();

        /// <returns>
        /// Raw json value retrieved by given <paramref name="key"/>.
        /// </returns>
        protected abstract string GetString(string key);

        /// <summary>
        /// Set raw json value retrieved at given <paramref name="key"/>.
        /// </summary>
        protected abstract void SetString(string key, string value);

        /// <summary>
        /// Delete raw json stored at given <paramref name="key"/>.
        /// </summary>
        protected abstract void DeleteString(string key);

        /// <returns>
        /// <code>json</code> string created from given <paramref name="obj"/>
        /// </returns>
        protected string SerializeObject<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, SerializerSettings);
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
