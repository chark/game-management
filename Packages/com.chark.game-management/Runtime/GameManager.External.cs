using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CHARK.GameManagement.Messaging;
using CHARK.GameManagement.Systems;
using Object = UnityEngine.Object;

namespace CHARK.GameManagement
{
    public abstract partial class GameManager
    {
        /// <returns>
        /// Resources of type <see cref="TResource"/> from <see cref="resourceLoader"/> at
        /// given <paramref name="path"/>.
        /// </returns>
        public static IEnumerable<TResource> GetResources<TResource>(string path = null) where TResource : Object
        {
            var gameManager = GetGameManager();
            var resourceLoader = gameManager.resourceLoader;

            return resourceLoader.GetResources<TResource>(path);
        }

        /// <returns>
        /// <c>true</c> if resource of type <see cref="TResource"/> is retrieved from
        /// <see cref="resourceLoader"/> at given <paramref name="path"/> or <c>false</c> otherwise.
        /// </returns>
        public static bool TryGetResource<TResource>(string path, out TResource resource) where TResource : Object
        {
            var gameManager = GetGameManager();
            var resourceLoader = gameManager.resourceLoader;

            return resourceLoader.TryGetResource(path, out resource);
        }

        /// <returns>
        /// Task containing a resource of type <see cref="TResource"/> retrieved from
        /// <see cref="resourceLoader"/>  at given <paramref name="path"/> or <c>default</c> if
        /// value could not be retrieved.
        /// </returns>
        public static Task<TResource> GetResourceAsync<TResource>(string path)
        {
            var gameManager = GetGameManager();
            var resourceLoader = gameManager.resourceLoader;

            return resourceLoader.GetResourceAsync<TResource>(path);
        }

        /// <returns>
        /// Value retrieved from <see cref="runtimeStorage"/> at given <paramref name="path"/>
        /// asynchronously or <c>default</c> if no value is could be retrieved.
        /// </returns>
        public static Task<TValue> GetRuntimeValueAsync<TValue>(string path)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.GetValueAsync<TValue>(path);
        }

        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is retrieved by given <paramref name="path"/>
        /// from <see cref="runtimeStorage"/>.
        /// </returns>
        public static bool TryGetRuntimeValue<TValue>(string path, out TValue value)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.TryGetValue(path, out value);
        }

        /// <summary>
        /// Persist a <paramref name="value"/> by given <paramref name="path"/> asynchronously to
        /// <see cref="runtimeStorage"/>.
        /// </summary>
        public static Task SetRuntimeValueAsync<TValue>(string path, TValue value)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.SetValueAsync(path, value);
        }

        /// <summary>
        /// Persist a <paramref name="value"/> by given <paramref name="path"/> to
        /// <see cref="runtimeStorage"/>.
        /// </summary>
        public static void SetRuntimeValue<TValue>(string path, TValue value)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            runtimeStorage.SetValue(path, value);
        }

        /// <summary>
        /// Delete persisted <see cref="runtimeStorage"/> value at given <paramref name="path"/>
        /// asynchronously from <see cref="runtimeStorage"/>.
        /// </summary>
        public static Task DeleteRuntimeValueAsync(string path)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.DeleteValueAsync(path);
        }

        /// <summary>
        /// Delete persisted <see cref="runtimeStorage"/> value at given <paramref name="path"/>
        /// from <see cref="runtimeStorage"/>.
        /// </summary>
        public static void DeleteRuntimeValue(string path)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            runtimeStorage.DeleteValue(path);
        }

        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is retrieved by given <paramref name="path"/>
        /// from <see cref="EditorStorage"/>.
        /// </returns>
        public static bool TryGetEditorValue<TValue>(string path, out TValue value)
        {
            return EditorStorage.TryGetValue(path, out value);
        }

        /// <summary>
        /// Persist a <paramref name="value"/> by given <paramref name="path"/> to
        /// <see cref="EditorStorage"/>.
        /// </summary>
        public static void SetEditorValue<TValue>(string path, TValue value)
        {
            EditorStorage.SetValue(path, value);
        }

        /// <summary>
        /// Delete persisted <see cref="EditorStorage"/> value at given <paramref name="path"/>.
        /// </summary>
        public static void DeleteEditorValue(string path)
        {
            EditorStorage.DeleteValue(path);
        }

        /// <returns>
        /// <c>true</c> if <paramref name="system"/> of type <see cref="TSystem"/> is retrieved
        /// from <see cref="entityManager"/> or <c>false</c> otherwise.
        /// </returns>
        public static bool TryGetSystem<TSystem>(out TSystem system) where TSystem : ISystem
        {
            var gameManager = GetGameManager();
            var entityManager = gameManager.entityManager;

            return entityManager.TryGetEntity(out system);
        }

        /// <returns>
        /// Enumerable of systems of type <see cref="TSystem"/> from <see cref="entityManager"/>.
        /// </returns>
        public static IEnumerable<TSystem> GetSystems<TSystem>() where TSystem : ISystem
        {
            var gameManager = GetGameManager();
            var entityManager = gameManager.entityManager;
            var entities = entityManager.GetEntities<TSystem>();

            return entities;
        }

        /// <returns>
        /// Systems of type <see cref="TSystem"/> from <see cref="entityManager"/>.
        /// </returns>
        public static TSystem GetSystem<TSystem>() where TSystem : ISystem
        {
            var gameManager = GetGameManager();
            var entityManager = gameManager.entityManager;

            return entityManager.GetEntity<TSystem>();
        }

        /// <summary>
        /// Publish a message to the <see cref="messageBus"/>.
        /// </summary>
        public static void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.Publish(message);
        }

        /// <summary>
        /// Add a listener to the <see cref="messageBus"/>.
        /// </summary>
        public static void AddListener<TMessage>(Action<TMessage> listener)
            where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.AddListener(listener);
        }

        /// <summary>
        /// Remove a listener from the <see cref="messageBus"/>.
        /// </summary>
        public static void RemoveListener<TMessage>(Action<TMessage> listener)
            where TMessage : IMessage
        {
            var gameManager = GetGameManager();
            var messageBus = gameManager.messageBus;

            messageBus.RemoveListener(listener);
        }

        /// <returns>
        /// <c>true</c> if <paramref name="serializedValue"/> is deserialized to
        /// <paramref name="deserializedValue"/> using <see cref="serializer"/> successfully or
        /// <c>false</c> otherwise.
        /// </returns>
        public static bool TryDeserializeValue<TValue>(string serializedValue, out TValue deserializedValue)
        {
            var gameManager = GetGameManager();
            var serializer = gameManager.serializer;

            return serializer.TryDeserializeValue(serializedValue, out deserializedValue);
        }

        /// <returns>
        /// <c>true</c> if <paramref name="deserializedValue"/> is serialized to
        /// <paramref name="serializedValue"/> using <see cref="serializer"/> successfully or
        /// <c>false</c> otherwise.
        /// </returns>
        public static bool TrySerializeValue<TValue>(TValue deserializedValue, out string serializedValue)
        {
            var gameManager = GetGameManager();
            var serializer = gameManager.serializer;

            return serializer.TrySerializeValue(deserializedValue, out serializedValue);
        }
    }
}
