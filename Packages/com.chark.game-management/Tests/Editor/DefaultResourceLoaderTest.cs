using System.IO;
using System.Threading.Tasks;
using CHARK.GameManagement.Assets;
using CHARK.GameManagement.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CHARK.GameManagement.Tests.Editor
{
    internal sealed class DefaultResourceLoaderTest
    {
        private DefaultResourceLoader resourceLoader;

        [SetUp]
        public void SetUp()
        {
            resourceLoader = new DefaultResourceLoader(DefaultSerializer.Instance);
        }

        [Test]
        public async Task ShouldReadResourceStreamFromStreamingAssetsAsync()
        {
            // When
            var stream = await resourceLoader.ReadResourceStreamAsync("TestData.txt");
            var length = stream.Length;

            string text;
            using (var reader = new StreamReader(stream))
            {
                text = await reader.ReadToEndAsync();
            }

            // Then
            Assert.Greater(length, 0);
            Assert.AreEqual(text, "hello");
        }

        [Test]
        public async Task ShouldReadAsyncResourceFromStreamingAssetsAsync()
        {
            // When
            var testData = await resourceLoader.ReadResourceAsync<TestData>("TestData.json");

            // Then
            Assert.AreEqual(testData.Message, "hello");
        }

        private readonly struct TestData
        {
            [JsonProperty("message")]
            public string Message { get; }

            [JsonConstructor]
            public TestData(string message)
            {
                Message = message;
            }
        }
    }
}
