using CHARK.GameManagement.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CHARK.GameManagement.Tests.Editor
{
    internal sealed class DefaultSerializerTest
    {
        private readonly ISerializer serializer = DefaultSerializer.Instance;

        [Test]
        public void ShouldSerializeString()
        {
            // Given
            const string value = "hello";

            // When
            if (serializer.TrySerializeValue(value, out var actualValue) == false)
            {
                Assert.Fail("Could not serialize string");
            }

            // Then
            Assert.AreEqual("hello", actualValue);
        }

        [Test]
        public void ShouldSerializeInt()
        {
            // Given
            const int value = 123;

            // When
            if (serializer.TrySerializeValue(value, out var actualValue) == false)
            {
                Assert.Fail("Could not serialize int");
            }

            // Then
            Assert.AreEqual("123", actualValue);
        }

        [Test]
        public void ShouldSerializeClass()
        {
            // Given
            var value = new TestClass("hello");

            // When
            if (serializer.TrySerializeValue(value, out var actualValue) == false)
            {
                Assert.Fail("Could not serialize class");
            }

            // Then
            Assert.AreEqual("{\"value\":\"hello\"}", actualValue);
        }

        [Test]
        public void ShouldDeserializeString()
        {
            // Given
            const string value = "hello";

            // When
            if (serializer.TryDeserializeValue<string>(value, out var actualValue) == false)
            {
                Assert.Fail("Could not deserialize string");
            }

            // Then
            Assert.AreEqual("hello", actualValue);
        }

        [Test]
        public void ShouldDeserializeInt()
        {
            // Given
            const string value = "123";

            // When
            if (serializer.TryDeserializeValue<int>(value, out var actualValue) == false)
            {
                Assert.Fail("Could not deserialize int");
            }

            // Then
            Assert.AreEqual(123, actualValue);
        }

        [Test]
        public void ShouldDeserializeClass()
        {
            // Given
            var value = "{\"value\":\"hello\"}";

            // When
            if (serializer.TryDeserializeValue<TestClass>(value, out var actualValue) == false)
            {
                Assert.Fail("Could not serialize class");
            }

            // Then
            Assert.AreEqual("hello", actualValue.Value);
        }

        private class TestClass
        {
            [JsonProperty]
            public string Value { get; }

            [JsonConstructor]
            public TestClass(string value)
            {
                Value = value;
            }
        }
    }
}
