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
        /// Resources of type <see cref="TResource"/> loaded via <see cref="resourceLoader"/> at
        /// given <paramref name="path"/>.
        /// </returns>
        public static IEnumerable<TResource> LoadResources<TResource>(string path = null) where TResource : Object
        {
            var gameManager = GetGameManager();
            var resourceLoader = gameManager.resourceLoader;

            return resourceLoader.LoadResources<TResource>(path);
        }

        /// <returns>
        /// Resource of type <see cref="TResource"/> loaded via <see cref="resourceLoader"/> at
        /// given <paramref name="path"/>. If resource can't be loaded an <see cref="System.Exception"/>
        /// is thrown.
        /// </returns>
        public static TResource LoadResource<TResource>(string path) where TResource : Object
        {
            var gameManager = GetGameManager();
            var resourceLoader = gameManager.resourceLoader;

            return resourceLoader.LoadResource<TResource>(path);
        }

        /// <returns>
        /// Value retrieved from <see cref="runtimeStorage"/> at given <paramref name="key"/>
        /// asynchronously or <c>default</c> if no value is could be retrieved.
        /// </returns>
        public static Task<TValue> GetRuntimeValueAsync<TValue>(string key)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.GetValueAsync<TValue>(key);
        }

        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is retrieved by given <paramref name="key"/>
        /// from <see cref="runtimeStorage"/>.
        /// </returns>
        public static bool TryGetRuntimeValue<TValue>(string key, out TValue value)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.TryGetValue(key, out value);
        }

        /// <summary>
        /// Persist a <paramref name="value"/> by given <paramref name="key"/> asynchronously to
        /// <see cref="runtimeStorage"/>.
        /// </summary>
        public static Task SetRuntimeValueAsync<TValue>(string key, TValue value)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            return runtimeStorage.SetValueAsync(key, value);
        }

        /// <summary>
        /// Persist a <paramref name="value"/> by given <paramref name="key"/> to
        /// <see cref="runtimeStorage"/>.
        /// </summary>
        public static void SetRuntimeValue<TValue>(string key, TValue value)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            runtimeStorage.SetValue(key, value);
        }

        // TODO: add async variant!
        /// <summary>
        /// Delete persisted <see cref="runtimeStorage"/>  value at given <paramref name="key"/>.
        /// </summary>
        public static void DeleteRuntimeValue(string key)
        {
            var gameManager = GetGameManager();
            var runtimeStorage = gameManager.runtimeStorage;

            runtimeStorage.DeleteValue(key);
        }

        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is retrieved by given <paramref name="key"/>
        /// from <see cref="EditorStorage"/>.
        /// </returns>
        public static bool TryGetEditorValue<TValue>(string key, out TValue value)
        {
            return EditorStorage.TryGetValue(key, out value);
        }

        /// <summary>
        /// Persist a <paramref name="value"/> by given <paramref name="key"/> to
        /// <see cref="EditorStorage"/>.
        /// </summary>
        public static void SetEditorValue<TValue>(string key, TValue value)
        {
            EditorStorage.SetValue(key, value);
        }

        /// <summary>
        /// Delete persisted <see cref="EditorStorage"/> value at given <paramref name="key"/>.
        /// </summary>
        public static void DeleteEditorValue(string key)
        {
            EditorStorage.DeleteValue(key);
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
    }
}
