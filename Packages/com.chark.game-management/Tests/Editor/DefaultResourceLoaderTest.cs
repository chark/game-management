using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CHARK.GameManagement.Assets;
using CHARK.GameManagement.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CHARK.GameManagement.Tests.Editor
{
    internal sealed class DefaultResourceLoaderTest
    {
        private DefaultResourceLoader resourceLoader;

        [SetUp]
        public void SetUp()
        {
            LogAssert.ignoreFailingMessages = true;
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
        public async Task ShouldReadMissingResourceStreamFromStreamingAssetsAsync()
        {
            // When
#if UNITY_ANDROID
            LogAssert.Expect(LogType.Exception, new Regex("HTTP/1.1 404 Not Found"));
#else
            LogAssert.Expect(LogType.Exception, new Regex("Could not find file"));
#endif

            var stream = await resourceLoader.ReadResourceStreamAsync("MissingData.txt");
            var length = stream.Length;

            // Then
            Assert.AreEqual(length, 0);
        }

        [Test]
        public async Task ShouldReadResourceFromStreamingAssetsAsync()
        {
            // When
            var testData = await resourceLoader.ReadResourceAsync<TestData>("TestData.json");

            // Then
            Assert.AreEqual(testData.Message, "hello");
        }

        [Test]
        public async Task ShouldReadMissingResourceFromStreamingAssetsAsync()
        {
            try
            {
                await resourceLoader.ReadResourceAsync<TestData>("MissingData.json");
            }
            catch (Exception exception)
            {
#if UNITY_ANDROID
                Assert.IsTrue(exception.Message.Contains("HTTP/1.1 404 Not Found"));
#else
                Assert.IsTrue(exception.Message.Contains("Could not find file"));
#endif
            }
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
