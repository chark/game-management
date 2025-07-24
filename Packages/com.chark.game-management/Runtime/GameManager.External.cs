using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CHARK.GameManagement.Assets;
using CHARK.GameManagement.Messaging;
using CHARK.GameManagement.Serialization;
using CHARK.GameManagement.Storage;
using CHARK.GameManagement.Systems;
using CHARK.GameManagement.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITASK_INSTALLED
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace CHARK.GameManagement
{
    public abstract partial class GameManager
    {
        public static bool IsDebuggingEnabled
        {
            get
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    return isDebuggingEnabled;
                }

                if (TryReadEditorData<bool>(nameof(IsDebuggingEnabled), out var value))
                {
                    return value;
                }
#endif
                return isDebuggingEnabled;
            }
            set
            {
                var oldValue = IsDebuggingEnabled;
                var newValue = value;

#if UNITY_EDITOR
                SaveEditorData(nameof(IsDebuggingEnabled), newValue);
#endif

                isDebuggingEnabled = newValue;

#if UNITY_EDITOR
                if (Application.isPlaying == false)
                {
                    return;
                }
#endif

                if (oldValue == newValue)
                {
                    return;
                }

                Publish(new DebuggingChangedMessage(newValue));
            }
        }

        /// <summary>
        /// Is the application quitting right now?
        /// </summary>
        public static bool IsApplicationQuitting => GameManagerUtilities.IsApplicationQuitting;

        /// <inheritdoc cref="IResourceLoader.GetResources{TResource}"/>
        public static IEnumerable<TResource> GetResources<TResource>(string path = null)
            where TResource : Object
        {
            var gameManager = GetGameManager();
            var resourceLoader = gameManager.resourceLoader;

            return resourceLoader.GetResources<TResource>(path);
        }

        /// <inheritdoc cref="IResourceLoader.TryGetResource{TResource}"/>
        public static bool TryGetResource<TResource>(string path, out TResource resource)
            where TResource : Object
        {
            if (TryGetGameManager(out var gameManager) == false)
            {
                resource = null;
                return false;
            }

            return gameManager.resourceLoader.TryGetResource(path, out resource);
        }

        /// <inheritdoc cref="IResourceLoader.ReadResourceAsync{TResource}"/>
#if UNITASK_INSTALLED
        public static UniTask<TResource> ReadResourceAsync<TResource>(
#else
        public static Task<TResource> ReadResourceAsync<TResource>(
#endif
            string path,
            CancellationToken cancellationToken = default
        )
        {
            var gameManager = GetGameManager();
            var resourceLoader = gameManager.resourceLoader;

            return resourceLoader.ReadResourceAsync<TResource>(path, cancellationToken);
        }

        /// <inheritdoc cref="IResourceLoader.ReadResourceStreamAsync"/>
#if UNITASK_INSTALLED
        public static UniTask<Stream> ReadResourceStreamAsync(
#else
        public static Task<Stream> ReadResourceStreamAsync(
#endif
            string path,
            CancellationToken cancellationToken = default
        )
        {
            var gameManager = GetGameManager();
            var resourceLoader = gameManager.resourceLoader;

            return resourceLoader.ReadResourceStreamAsync(path, cancellationToken);
        }

        /// <inheritdoc cref="IStorage.TryReadData{TData}"/>
        public static bool TryReadData<TData>(string path, out TData data)
        {
            if (TryGetGameManager(out var gameManager) == false)
            {
                data = default;
                return false;
            }

            return gameManager.runtimeStorage.TryReadData(path, out data);
        }

        /// <inheritdoc cref="IStorage.ReadDataAsync{TData}"/>
#if UNITASK_INSTALLED
        public static UniTask<TData> ReadDataAsync<TData>(
#else
        public static Task<TData> ReadDataAsync<TData>(
#endif
            string path,
            CancellationToken cancellationToken = default
        )
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.ReadDataAsync<TData>(path, cancellationToken);
        }

        /// <inheritdoc cref="IStorage.ReadDataStream"/>
        public static Stream ReadDataStream(string path)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.ReadDataStream(path);
        }

        /// <inheritdoc cref="IStorage.ReadDataStreamAsync"/>
#if UNITASK_INSTALLED
        public static UniTask<Stream> ReadDataStreamAsync(
#else
        public static Task<Stream> ReadDataStreamAsync(
#endif
            string path,
            CancellationToken cancellationToken = default
        )
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.ReadDataStreamAsync(path, cancellationToken);
        }

        /// <inheritdoc cref="IStorage.SaveData{TData}"/>
        public static void SaveData<TData>(string path, TData data)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            runtimeStorage.SaveData(path, data);
        }

        /// <inheritdoc cref="IStorage.SaveDataStream"/>
        public static void SaveDataStream(string path, Stream stream)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            runtimeStorage.SaveDataStream(path, stream);
        }

        /// <inheritdoc cref="IStorage.SaveDataAsync{TData}"/>
#if UNITASK_INSTALLED
        public static UniTask SaveDataAsync<TData>(
#else
        public static Task SaveDataAsync<TData>(
#endif
            string path,
            TData data,
            CancellationToken cancellationToken = default
        )
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.SaveDataAsync(path, data, cancellationToken);
        }

        /// <inheritdoc cref="IStorage.SaveDataStreamAsync"/>
#if UNITASK_INSTALLED
        public static UniTask SaveDataStreamAsync(
#else
        public static Task SaveDataStreamAsync(
#endif
            string path,
            Stream stream,
            CancellationToken cancellationToken = default
        )
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.SaveDataStreamAsync(path, stream, cancellationToken);
        }

        /// <inheritdoc cref="IStorage.DeleteData"/>
        public static void DeleteData(string path)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            runtimeStorage.DeleteData(path);
        }

        /// <inheritdoc cref="IStorage.DeleteDataAsync"/>
#if UNITASK_INSTALLED
        public static UniTask DeleteDataAsync(
#else
        public static Task DeleteDataAsync(
#endif
            string path,
            CancellationToken cancellationToken = default
        )
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.DeleteDataAsync(path, cancellationToken);
        }

        /// <inheritdoc cref="IStorage.TryReadData{TData}"/>
        /// <remarks>
        /// This method should only be used in Editor, it will not function in builds.
        /// </remarks>
        public static bool TryReadEditorData<TData>(string path, out TData data)
        {
            return EditorStorage.TryReadData(path, out data);
        }

        /// <inheritdoc cref="IStorage.SaveData{TData}"/>
        /// <remarks>
        /// This method should only be used in Editor, it will not function in builds.
        /// </remarks>
        public static void SaveEditorData<TData>(string path, TData data)
        {
            EditorStorage.SaveData(path, data);
        }

        /// <inheritdoc cref="IStorage.DeleteData"/>
        /// <remarks>
        /// This method should only be used in Editor, it will not function in builds.
        /// </remarks>
        public static void DeleteEditorData(string path)
        {
            EditorStorage.DeleteData(path);
        }

        /// <returns>
        /// <c>true</c> if <paramref name="system"/> of type <see cref="TSystem"/> is retrieved
        /// from <see cref="entityManager"/> or <c>false</c> otherwise.
        /// </returns>
        public static bool TryGetSystem<TSystem>(out TSystem system) where TSystem : ISystem
        {
            if (TryGetGameManager(out var gameManager) == false)
            {
                system = default;
                return false;
            }

            return gameManager.entityManager.TryGetEntity(out system);
        }

        /// <returns>
        /// Enumerable of systems of type <see cref="TSystem"/> retrieved from
        /// <see cref="entityManager"/>.
        /// </returns>
        public static IEnumerable<TSystem> GetSystems<TSystem>() where TSystem : ISystem
        {
            var gameManager = GetGameManager();
            var entityManager = gameManager.entityManager;
            var entities = entityManager.GetEntities<TSystem>();

            return entities;
        }

        /// <returns>
        /// System of type <see cref="TSystem"/> retrieved from <see cref="entityManager"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// if system of type <see cref="TSystem"/> is not found.
        /// </exception>
        public static TSystem GetSystem<TSystem>() where TSystem : ISystem
        {
            var gameManager = GetGameManager();
            var entityManager = gameManager.entityManager;

            return entityManager.GetEntity<TSystem>();
        }

        /// <inheritdoc cref="IMessageBus.Publish{TMessage}"/>
        public static void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.Publish(message);
        }

        /// <inheritdoc cref="IMessageBus.AddListener{TMessage}(CHARK.GameManagement.Messaging.OnMessageReceived{TMessage})"/>
        public static void AddListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.AddListener(listener);
        }

        /// <inheritdoc cref="IMessageBus.AddListener{TMessage}(CHARK.GameManagement.Messaging.OnMessageReceivedAsync{TMessage})"/>
        public static void AddListener<TMessage>(OnMessageReceivedAsync<TMessage> listener) where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.AddListener(listener);
        }

        /// <inheritdoc cref="IMessageBus.AddListener{TMessage}(CHARK.GameManagement.Messaging.OnMessageReceivedCancellableAsync{TMessage})"/>
        public static void AddListener<TMessage>(OnMessageReceivedCancellableAsync<TMessage> listener) where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.AddListener(listener);
        }

        /// <inheritdoc cref="IMessageBus.RemoveListener{TMessage}(CHARK.GameManagement.Messaging.OnMessageReceived{TMessage})"/>
        public static void RemoveListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.RemoveListener(listener);
        }

        /// <inheritdoc cref="IMessageBus.RemoveListener{TMessage}(CHARK.GameManagement.Messaging.OnMessageReceivedAsync{TMessage})"/>
        public static void RemoveListener<TMessage>(OnMessageReceivedAsync<TMessage> listener) where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.RemoveListener(listener);
        }

        /// <inheritdoc cref="IMessageBus.RemoveListener{TMessage}(CHARK.GameManagement.Messaging.OnMessageReceivedAsync{TMessage})"/>
        public static void RemoveListener<TMessage>(OnMessageReceivedCancellableAsync<TMessage> listener) where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.RemoveListener(listener);
        }

        /// <inheritdoc cref="ISerializer.TryDeserializeValue{TValue}"/>
        public static bool TryDeserializeValue<TValue>(string value, out TValue deserializedValue)
        {
            if (TryGetGameManager(out var gameManager) == false)
            {
                deserializedValue = default;
                return false;
            }

            return gameManager.serializer.TryDeserializeValue(value, out deserializedValue);
        }

        /// <inheritdoc cref="ISerializer.TrySerializeValue{TValue}"/>
        public static bool TrySerializeValue<TValue>(TValue value, out string serializedValue)
        {
            if (TryGetGameManager(out var gameManager) == false)
            {
                serializedValue = string.Empty;
                return false;
            }

            return gameManager.serializer.TrySerializeValue(value, out serializedValue);
        }

        /// <returns>
        /// Logger which formats messages in a consistent format using <paramref name="contextObject"/>
        /// as context in log messages.
        /// </returns>
        public static GameLogger LogWith(object contextObject)
        {
            if (contextObject is Object unityContextObject && unityContextObject)
            {
                return LogWith(unityContextObject);
            }

            if (contextObject != null)
            {
                return LogWith(contextObject.GetType());
            }

            return new GameLogger(typeof(GameLogger));
        }

        /// <returns>
        /// Logger which formats messages in a consistent format using <paramref name="contextObject"/>
        /// as context in log messages.
        /// </returns>
        public static GameLogger LogWith(Object contextObject)
        {
            return new GameLogger(contextObject);
        }

        /// <returns>
        /// Logger which formats messages in a consistent format using <paramref name="contextType"/>
        /// as context in log messages.
        /// </returns>
        public static GameLogger LogWith(Type contextType)
        {
            return new GameLogger(contextType);
        }
    }
}
