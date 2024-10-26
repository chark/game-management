using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CHARK.GameManagement.Actors;
using CHARK.GameManagement.Assets;
using CHARK.GameManagement.Messaging;
using CHARK.GameManagement.Serialization;
using CHARK.GameManagement.Storage;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CHARK.GameManagement
{
    public abstract partial class GameManager
    {
        /// <summary>
        /// <c>true</c> if debug mode is enabled or <c>false</c> otherwise. When this value changes, a
        /// <see cref="DebuggingChangedMessage"/> is raised.
        /// </summary>
        public static bool IsDebuggingEnabled
        {
            get
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    return isDebuggingEnabled;
                }

                if (TryReadEditorData<bool>(IsDebuggingEnabledKey, out var value))
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
                SaveEditorData(IsDebuggingEnabledKey, newValue);
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

                var message = new DebuggingChangedMessage(oldValue, newValue);
                Publish(message);
            }
        }

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
            var gameManager = GetGameManager();
            var resourceLoader = gameManager.resourceLoader;

            return resourceLoader.TryGetResource(path, out resource);
        }

        /// <inheritdoc cref="IResourceLoader.ReadResourceAsync{TResource}"/>
        public static Task<TResource> ReadResourceAsync<TResource>(
            string path,
            CancellationToken cancellationToken = default
        )
        {
            var gameManager = GetGameManager();
            var resourceLoader = gameManager.resourceLoader;

            return resourceLoader.ReadResourceAsync<TResource>(path, cancellationToken);
        }

        /// <inheritdoc cref="IResourceLoader.ReadResourceStreamAsync"/>
        public static Task<Stream> ReadResourceStreamAsync(
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
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.TryReadData(path, out data);
        }

        /// <inheritdoc cref="IStorage.ReadDataAsync{TData}"/>
        public static Task<TData> ReadDataAsync<TData>(
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
        public static Task<Stream> ReadDataStreamAsync(
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
        public static Task SaveDataAsync<TData>(
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
        public static Task SaveDataStreamAsync(
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
        public static Task DeleteDataAsync(
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
        /// <c>true</c> if <paramref name="actor"/> of the given type is retrieved
        /// from <see cref="actorManager"/> or <c>false</c> otherwise.
        /// </returns>
        public static bool TryGetActor<TActor>(out TActor actor) where TActor : IActor
        {
            var gameManager = GetGameManager();
            var actorManager = gameManager.actorManager;

            return actorManager.TryGetActor(out actor);
        }

        /// <returns>
        /// Enumerable containing actors of the given type, retrieved from <see cref="actorManager"/>.
        /// </returns>
        public static IEnumerable<TActor> GetActors<TActor>() where TActor : IActor
        {
            var gameManager = GetGameManager();
            var actorManager = gameManager.actorManager;

            return actorManager.GetActors<TActor>();
        }

        /// <returns>
        /// Actor of given type, retrieved from <see cref="actorManager"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// if the actor of given type is not found.
        /// </exception>
        public static TActor GetActor<TActor>() where TActor : IActor
        {
            var gameManager = GetGameManager();
            var actorManager = gameManager.actorManager;

            return actorManager.GetActor<TActor>();
        }

        /// <summary>
        /// Add <paramref name="actor"/> of the given type from the <see cref="actorManager"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if given <paramref name="actor"/> is added or <c>false</c> otherwise.
        /// </returns>
        public static bool AddActor<TActor>(TActor actor) where TActor : IActor
        {
            var gameManager = GetGameManager();
            var actorManager = gameManager.actorManager;

            return actorManager.AddActor(actor);
        }

        /// <summary>
        /// Remove <paramref name="actor"/> of the given type from the <see cref="actorManager"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if given <paramref name="actor"/> is removed or <c>false</c> otherwise.
        /// </returns>
        public static bool RemoveActor<TActor>(TActor actor) where TActor : IActor
        {
            var gameManager = GetGameManager();
            var actorManager = gameManager.actorManager;

            return actorManager.RemoveActor(actor);
        }

        /// <inheritdoc cref="IMessageBus.Publish{TMessage}"/>
        public static void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.Publish(message);
        }

        /// <inheritdoc cref="IMessageBus.AddListener{TMessage}"/>
        public static void AddListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.AddListener(listener);
        }

        /// <inheritdoc cref="IMessageBus.RemoveListener{TMessage}"/>
        public static void RemoveListener<TMessage>(OnMessageReceived<TMessage> listener) where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.RemoveListener(listener);
        }

        /// <inheritdoc cref="ISerializer.TryDeserializeValue{TValue}"/>
        public static bool TryDeserializeValue<TValue>(string value, out TValue deserializedValue)
        {
            var gameManager = GetGameManager();
            var serializer = gameManager.serializer;

            return serializer.TryDeserializeValue(value, out deserializedValue);
        }

        /// <inheritdoc cref="ISerializer.TrySerializeValue{TValue}"/>
        public static bool TrySerializeValue<TValue>(TValue value, out string serializedValue)
        {
            var gameManager = GetGameManager();
            var serializer = gameManager.serializer;

            return serializer.TrySerializeValue(value, out serializedValue);
        }
    }
}
