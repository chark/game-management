using System.IO;

namespace CHARK.GameManagement.Serialization
{
    public interface ISerializer
    {
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is deserialized to
        /// <paramref name="deserializedValue"/> or <c>false</c> otherwise.
        /// </returns>
        public bool TryDeserializeValue<TValue>(string value, out TValue deserializedValue);

        /// <returns>
        /// <c>true</c> if <paramref name="stream"/> is deserialized to
        /// <paramref name="deserializedValue"/> or <c>false</c> otherwise.
        /// </returns>
        public bool TryDeserializeStream<TValue>(Stream stream, out TValue deserializedValue);

        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is serialized to
        /// <paramref name="serializedValue"/> or <c>false</c> otherwise.
        /// </returns>
        public bool TrySerializeValue<TValue>(TValue value, out string serializedValue);

        /// <returns>
        /// <c>true</c> if <paramref name="stream"/> is serialized to
        /// <paramref name="serializedValue"/> or <c>false</c> otherwise.
        /// </returns>
        public bool TrySerializeStream(Stream stream, out string serializedValue);
    }
}
