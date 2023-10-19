using System;
using System.IO;
using CHARK.GameManagement.Assets;
using CHARK.GameManagement.Entities;
using CHARK.GameManagement.Messaging;
using CHARK.GameManagement.Serialization;
using CHARK.GameManagement.Settings;
using CHARK.GameManagement.Storage;
using CHARK.GameManagement.Systems;
using CHARK.GameManagement.Utilities;
using UnityEngine;

namespace CHARK.GameManagement
{
    [DefaultExecutionOrder(-1)]
    public abstract partial class GameManager : MonoBehaviour
    {
        private static GameManager currentGameManager;

        private static readonly IStorage EditorStorage =
#if UNITY_EDITOR
            new EditorPrefsStorage(DefaultSerializer.Instance, $"{nameof(GameManager)}.");
#else
            NullStorage.Instance;
#endif

        private static GameManagerSettings Settings => GameManagerSettings.Instance;

        private bool isSystemsInitialized;

        private ISerializer serializer;
        private IStorage runtimeStorage;
        private IResourceLoader resourceLoader;
        private IEntityManager entityManager;
        private IMessageBus messageBus;

        private void Awake()
        {
            if (currentGameManager && currentGameManager != this)
            {
                var gameManagerTypeName = GetType().Name;
                Logging.LogWarning($"{gameManagerTypeName} is already initialized", this);
                Destroy(gameObject);
                return;
            }

            var isInitialized = currentGameManager == true;

            currentGameManager = this;

            if (isInitialized == false)
            {
                InitializeGameManager();

                var profile = Settings.ActiveProfile;
                if (profile.IsVerboseLogging)
                {
                    Logging.LogDebug($"{GetGameManagerName()} initialized", this);
                }

                if (profile.IsDontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }

        private void FixedUpdate()
        {
            NotifyFixedUpdateListeners();
        }

        private void Update()
        {
            NotifyUpdateListeners();
        }

        private void OnDestroy()
        {
            OnBeforeDestroy();

            if (entityManager != null)
            {
                var systems = entityManager.GetEntities<ISystem>();
                foreach (var system in systems)
                {
                    RemoveSystem(system);
                }
            }

            var profile = Settings.ActiveProfile;
            if (profile.IsVerboseLogging)
            {
                Logging.LogDebug($"{GetGameManagerName()} disposed", this);
            }
        }

        /// <summary>
        /// Called when systems are about to initialize and should be added to the game.
        /// </summary>
        protected virtual void OnBeforeInitializeSystems()
        {
        }

        /// <summary>
        /// Called when all systems are initialized.
        /// </summary>
        protected virtual void OnAfterInitializeSystems()
        {
        }

        /// <summary>
        /// Called the game manager is about to be destroyed.
        /// </summary>
        protected virtual void OnBeforeDestroy()
        {
        }

        /// <returns>
        /// Name of this game manager.
        /// </returns>
        protected virtual string GetGameManagerName()
        {
            return GetType().Name;
        }

        /// <summary>
        /// Add <paramref name="system"/> to <see cref="entityManager"/>.
        /// </summary>
        protected void AddSystem(ISystem system)
        {
            if (entityManager.AddEntity(system) && isSystemsInitialized)
            {
                system.OnInitialized();
            }
        }

        /// <summary>
        /// Remove <paramref name="system"/> from <see cref="entityManager"/>.
        /// </summary>
        protected void RemoveSystem(ISystem system)
        {
            if (entityManager.RemoveEntity(system))
            {
                system.OnDisposed();
            }
        }

        /// <returns>
        /// New <see cref="ISerializer"/> instance.
        /// </returns>
        protected virtual ISerializer CreateSerializer()
        {
            return DefaultSerializer.Instance;
        }

        /// <returns>
        /// New <see cref="IStorage"/> instance.
        /// </returns>
        protected virtual IStorage CreateRuntimeStorage()
        {
            return new FileStorage(
                serializer: serializer,
                profile: Settings.ActiveProfile,
                persistentDataPath: Application.persistentDataPath,
                pathPrefix: "Data" + Path.DirectorySeparatorChar
            );
        }

        /// <returns>
        /// New <see cref="IResourceLoader"/> instance.
        /// </returns>
        protected virtual IResourceLoader CreateResourceLoader()
        {
            return new ResourceLoader(serializer);
        }

        /// <returns>
        /// New <see cref="IEntityManager"/> instance.
        /// </returns>
        protected virtual IEntityManager CreateEntityManager()
        {
            var profile = Settings.ActiveProfile;
            return new EntityManager(profile);
        }

        /// <returns>
        /// New <see cref="IMessageBus"/> instance.
        /// </returns>
        protected virtual IMessageBus CreateMessageBus()
        {
            return new MessageBus();
        }

        private void InitializeGameManager()
        {
            InitializeCore();

            OnBeforeInitializeSystems();
            InitializeSystems();
            OnAfterInitializeSystems();
        }

        private void InitializeCore()
        {
            name = GetGameManagerName();
            serializer = CreateSerializer();
            runtimeStorage = CreateRuntimeStorage();
            resourceLoader = CreateResourceLoader();
            entityManager = CreateEntityManager();
            messageBus = CreateMessageBus();
        }

        private void InitializeSystems()
        {
            var entities = entityManager.Entities;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < entities.Count; index++)
            {
                var entity = entities[index];
                if (!(entity is ISystem system))
                {
                    continue;
                }

                var profile = Settings.ActiveProfile;
                if (profile.IsVerboseLogging)
                {
                    var systemType = entity.GetType();
                    var systemName = systemType.Name;

                    Logging.LogDebug($"Initializing system {systemName}", this);
                }

                system.OnInitialized();
            }

            isSystemsInitialized = true;
        }

        private static GameManager GetGameManager()
        {
#if UNITY_EDITOR
            if (GameManagerUtilities.IsApplicationQuitting)
            {
                // If application is quitting, we don't care about warnings.
                return currentGameManager;
            }

            if (currentGameManager == false)
            {
                throw new Exception(
                    $"{nameof(GameManager)} is not initialized."
                    + $" Did you forget to add it to one of your scenes?"
                    + $" If you're using automatic instantiation "
                    + $" ({nameof(GameManagerSettingsProfile.IsInstantiateAutomatically)}),"
                    + $" make sure the {nameof(GameManagerSettings)} contains a valid and active"
                    + $" profile with a set {nameof(GameManager)} prefab."
                );
            }
#endif

            return currentGameManager;
        }

        private void NotifyFixedUpdateListeners()
        {
            var deltaTime = Time.deltaTime;
            var entities = entityManager.Entities;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < entities.Count; index++)
            {
                var entity = entities[index];
                if (entity is IFixedUpdateListener fixedUpdateListener)
                {
                    fixedUpdateListener.OnFixedUpdated(deltaTime);
                }
            }
        }

        private void NotifyUpdateListeners()
        {
            var deltaTime = Time.deltaTime;
            var entities = entityManager.Entities;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < entities.Count; index++)
            {
                var entity = entities[index];
                if (entity is IUpdateListener updateListener)
                {
                    updateListener.OnUpdated(deltaTime);
                }
            }
        }
    }
}
