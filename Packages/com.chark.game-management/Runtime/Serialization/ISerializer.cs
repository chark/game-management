namespace CHARK.GameManagement.Serialization
{
    public interface ISerializer
    {
        /// <returns>
        /// <c>true</c> if <paramref name="deserializedValue"/> is retrieved from
        /// <paramref name="serializedValue"/> or <c>false</c> otherwise.
        /// </returns>
        public bool TryDeserializeValue<TValue>(string serializedValue, out TValue deserializedValue);

        /// <returns>
        /// <c>true</c> if <paramref name="serializedValue"/> is retrieved from
        /// <paramref name="deserializedValue"/> or <c>false</c> otherwise.
        /// </returns>
        public bool TrySerializeValue<TValue>(TValue deserializedValue, out string serializedValue);
    }
}
