using System.IO;
using System.Threading.Tasks;
using CHARK.GameManagement.Assets;
using CHARK.GameManagement.Serialization;
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
        public async Task ShouldReadFileFromStreamingAssets()
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
    }
}
