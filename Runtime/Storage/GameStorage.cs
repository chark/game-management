using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace CHARK.GameManagement.Storage
{
    internal abstract class GameStorage : IGameStorage
    {
        private readonly JsonSerializerSettings jsonSerializerSettings;
        private readonly string keyPrefix;

        protected GameStorage(string keyPrefix) : this(keyPrefix, CreateDefaultSerializerSettings())
        {
        }

        protected GameStorage(string keyPrefix, JsonSerializerSettings jsonSerializerSettings)
        {
            this.keyPrefix = keyPrefix;
            this.jsonSerializerSettings = jsonSerializerSettings;
        }

        public async Task<TValue> GetValueAsync<TValue>(string key)
        {
            await UniTask.SwitchToThreadPool();

            if (TryGetValue<TValue>(key, out var value))
            {
                await UniTask.SwitchToMainThread();

                return value;
            }

            return default;
        }

        public virtual bool TryGetValue<TValue>(string key, out TValue value)
        {
            value = default;

            if (TryGetFormattedKey(key, out var formattedKey) == false)
            {
                return false;
            }

            var stringValue = GetString(formattedKey);
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            if (TryDeserializeConverterValue(stringValue, out TValue converterValue))
            {
                value = converterValue;
                return true;
            }

            if (TryDeserializeJsonValue(stringValue, out TValue jsonValue))
            {
                value = jsonValue;
                return true;
            }

            return true;
        }

        public async Task SetValueAsync<TValue>(string key, TValue value)
        {
            await UniTask.SwitchToThreadPool();
            SetValue(key, value);
            await UniTask.SwitchToMainThread();
        }

        public virtual void SetValue<TValue>(string key, TValue value)
        {
            if (TryGetFormattedKey(key, out var formattedKey) == false)
            {
                return;
            }

            if (TrySerializeValue(value, out var stringValue) == false)
            {
                return;
            }

            SetString(formattedKey, stringValue);
        }

        public void DeleteValue(string key)
        {
            if (TryGetFormattedKey(key, out var formattedKey))
            {
                DeleteKey(formattedKey);
            }
        }

        /// <returns>
        /// String retrieved using given <paramref name="key"/>.
        /// </returns>
        protected abstract string GetString(string key);

        /// <summary>
        /// Store a <paramref name="value"/> at given <paramref name="key"/>.
        /// </summary>
        protected abstract void SetString(string key, string value);

        /// <summary>
        /// Delete value at given <paramref name="key"/>.
        /// </summary>
        protected abstract void DeleteKey(string key);

        private bool TryGetFormattedKey(string key, out string formattedKey)
        {
            formattedKey = default;

            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            formattedKey = keyPrefix + key;
            return true;
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

        private bool TryDeserializeJsonValue<TValue>(string json, out TValue deserializedValue)
        {
            deserializedValue = default;

            try
            {
                var jsonValue = JsonConvert.DeserializeObject<TValue>(json, jsonSerializerSettings);
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

        private bool TrySerializeValue<TValue>(TValue value, out string serializedJson)
        {
            serializedJson = default;

            var valueType = typeof(TValue);
            if (valueType.IsPrimitive || valueType == typeof(string))
            {
                var valueString = value.ToString();
                serializedJson = valueString;
                return true;
            }

            try
            {
                var valueJson = JsonConvert.SerializeObject(value, jsonSerializerSettings);
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

        private static JsonSerializerSettings CreateDefaultSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}
