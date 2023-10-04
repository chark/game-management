using CHARK.GameManagement.Assets;
using CHARK.GameManagement.Entities;
using CHARK.GameManagement.Messaging;
using CHARK.GameManagement.Storage;
using CHARK.GameManagement.Systems;
using UnityEngine;

namespace CHARK.GameManagement
{
    [DefaultExecutionOrder(-1)]
    public abstract partial class GameManager : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Features", Expanded = true)]
#else
        [Header("Features")]
#endif
        [SerializeField]
        private bool isDontDestroyOnLoad = true;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Features", Expanded = true)]
#endif
        [SerializeField]
        private bool isVerboseLogging;

        private static GameManager currentGameManager;

        private static readonly IGameStorage EditorStorage =
#if UNITY_EDITOR
            new EditorPrefsGameStorage($"{nameof(GameManager)}.");
#else
            DefaultGameStorage.Instance;
#endif

        private bool isSystemsInitialized;

        private IGameStorage runtimeStorage;
        private IResourceLoader resourceLoader;
        private IEntityManager entityManager;
        private IMessageBus messageBus;

        private void Awake()
        {
            if (currentGameManager && currentGameManager != this)
            {
                var gameManagerTypeName = GetType().Name;
                Debug.LogWarning($"{gameManagerTypeName} is already initialized", this);
                Destroy(gameObject);
                return;
            }

            var isInitialized = currentGameManager == true;

            currentGameManager = this;

            if (isInitialized == false)
            {
                InitializeGameManager();

                if (isDontDestroyOnLoad)
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

        /// <summary>
        /// Called when systems are about to initialize and should be added to the game.
        /// </summary>
        protected abstract void OnBeforeInitializeSystems();

        /// <summary>
        /// Called when all systems are initialized.
        /// </summary>
        protected abstract void OnAfterInitializeSystems();

        /// <returns>
        /// Name of this game manager.
        /// </returns>
        protected abstract string GetGameManagerName();

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
        /// New <see cref="IGameStorage"/> instance.
        /// </returns>
        protected virtual IGameStorage CreateRuntimeGameStorage()
        {
            var keyPrefix = $"{GetGameManagerName()}.";
            return new FileGameStorage(keyPrefix, Application.persistentDataPath);
        }

        /// <returns>
        /// New <see cref="IResourceLoader"/> instance.
        /// </returns>
        protected virtual IResourceLoader CreateResourceLoader()
        {
            return new ResourceLoader();
        }

        /// <returns>
        /// New <see cref="IEntityManager"/> instance.
        /// </returns>
        protected virtual IEntityManager CreateEntityManager()
        {
            return new EntityManager(isVerboseLogging);
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
            runtimeStorage = CreateRuntimeGameStorage();
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

                if (isVerboseLogging)
                {
                    var systemType = entity.GetType();
                    var systemName = systemType.Name;

                    Debug.Log($"Initializing system {systemName}", this);
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
                Debug.LogError($"{nameof(GameManager)} is not initialized");
                Debug.Break();
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
