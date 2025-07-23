using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CHARK.GameManagement.Serialization
{
    internal class DefaultSerializer : ISerializer
    {
        internal static DefaultSerializer Instance { get; } = new();

        private readonly JsonSerializer jsonSerializer = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        private DefaultSerializer()
        {
        }

        public bool TryDeserializeValue<TValue>(string value, out TValue deserializedValue)
        {
            if (TryDeserializeConverterValue(value, out TValue converterValue))
            {
                deserializedValue = converterValue;
                return true;
            }

            deserializedValue = default;

            try
            {
                var jsonValue = JsonConvert.DeserializeObject<TValue>(value);
                if (jsonValue == null)
                {
                    return false;
                }

                deserializedValue = jsonValue;
                return true;
            }
            catch (Exception exception)
            {
                GameManager.LogWith(GetType()).LogError(exception);
                return false;
            }
        }

        public bool TryDeserializeStream<TValue>(Stream stream, out TValue deserializedValue)
        {
            if (TryDeserializeConverterValue(stream, out TValue converterValue))
            {
                deserializedValue = converterValue;
                return true;
            }

            deserializedValue = default;

            try
            {
                using var streamReader = new StreamReader(stream);
                using var jsonReader = new JsonTextReader(streamReader);

                var jsonValue = jsonSerializer.Deserialize<TValue>(jsonReader);
                if (jsonValue == null)
                {
                    return false;
                }

                deserializedValue = jsonValue;
                return true;
            }
            catch (Exception exception)
            {
                GameManager.LogWith(GetType()).LogError(exception);
                return false;
            }
        }

        public bool TrySerializeValue<TValue>(TValue value, out string serializedValue)
        {
            serializedValue = default;

            var valueType = typeof(TValue);
            if (valueType.IsPrimitive || valueType == typeof(string))
            {
                var valueString = value.ToString();
                serializedValue = valueString;
                return true;
            }

            try
            {
                using var stringWriter = new StringWriter();
                jsonSerializer.Serialize(stringWriter, value);

                var valueJson = stringWriter.ToString();
                if (string.IsNullOrWhiteSpace(valueJson))
                {
                    return false;
                }

                serializedValue = valueJson;
                return true;
            }
            catch (Exception exception)
            {
                GameManager.LogWith(GetType()).LogError(exception);
                return false;
            }
        }

        public bool TrySerializeStream(Stream stream, out string serializedValue)
        {
            serializedValue = default;

            try
            {
                using var memoryStream = new MemoryStream();
                using var streamWriter = new StreamWriter(memoryStream);
                using var jsonWriter = new JsonTextWriter(streamWriter);

                jsonSerializer.Serialize(jsonWriter, stream);

                var valueJson = Encoding.UTF8.GetString(memoryStream.ToArray());
                if (string.IsNullOrWhiteSpace(valueJson))
                {
                    return false;
                }

                serializedValue = valueJson;
                return true;
            }
            catch (Exception exception)
            {
                GameManager.LogWith(GetType()).LogError(exception);
                return false;
            }
        }

        private static bool TryDeserializeConverterValue<TValue>(Stream stream, out TValue deserializedValue)
        {
            deserializedValue = default;

            var valueType = typeof(TValue);
            if (valueType.IsPrimitive == false && valueType != typeof(string))
            {
                return false;
            }

            try
            {
                var converter = TypeDescriptor.GetConverter(valueType);
                var streamReader = new StreamReader(stream);
                var json = streamReader.ReadToEnd();

                var converterValue = (TValue)converter.ConvertFrom(json);
                if (converterValue == null)
                {
                    return false;
                }

                deserializedValue = converterValue;
                return true;
            }
            catch (Exception exception)
            {
                GameManager.LogWith(typeof(DefaultSerializer)).LogError(exception);
                return false;
            }
        }

        private static bool TryDeserializeConverterValue<TValue>(string json, out TValue deserializedValue)
        {
            deserializedValue = default;

            var valueType = typeof(TValue);
            if (valueType.IsPrimitive == false && valueType != typeof(string))
            {
                return false;
            }

            try
            {
                var converter = TypeDescriptor.GetConverter(valueType);
                var converterValue = (TValue)converter.ConvertFrom(json);
                if (converterValue == null)
                {
                    return false;
                }

                deserializedValue = converterValue;
                return true;
            }
            catch (Exception exception)
            {
                GameManager.LogWith(typeof(DefaultSerializer)).LogError(exception);
                return false;
            }
        }
    }
}
