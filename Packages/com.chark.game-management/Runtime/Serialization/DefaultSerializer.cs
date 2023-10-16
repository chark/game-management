using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace CHARK.GameManagement.Serialization
{
    internal class DefaultSerializer : ISerializer
    {
        internal static DefaultSerializer Instance { get; } = new();

        private readonly JsonSerializerSettings serializerSettings;

        private DefaultSerializer()
        {
            serializerSettings = CreateDefaultSerializerSettings();
        }

        public bool TryDeserializeValue<TValue>(string serializedValue, out TValue deserializedValue)
        {
            if (TryDeserializeConverterValue(serializedValue, out TValue converterValue))
            {
                deserializedValue = converterValue;
                return true;
            }

            deserializedValue = default;

            try
            {
                var jsonValue = JsonConvert.DeserializeObject<TValue>(serializedValue);
                if (jsonValue == null)
                {
                    return false;
                }

                deserializedValue = jsonValue;
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return false;
            }
        }

        public bool TrySerializeValue<TValue>(TValue deserializedValue, out string serializedJson)
        {
            serializedJson = default;

            var valueType = typeof(TValue);
            if (valueType.IsPrimitive || valueType == typeof(string))
            {
                var valueString = deserializedValue.ToString();
                serializedJson = valueString;
                return true;
            }

            try
            {
                var valueJson = JsonConvert.SerializeObject(deserializedValue, serializerSettings);
                if (string.IsNullOrWhiteSpace(valueJson))
                {
                    return false;
                }

                serializedJson = valueJson;
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                throw;
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
                Debug.LogException(exception);
                return false;
            }
        }

        private static JsonSerializerSettings CreateDefaultSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
        }
    }
}
