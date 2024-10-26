using System;
using System.IO;
using CHARK.GameManagement.Actors;
using CHARK.GameManagement.Assets;
using CHARK.GameManagement.Messaging;
using CHARK.GameManagement.Serialization;
using CHARK.GameManagement.Settings;
using CHARK.GameManagement.Storage;
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

        private const string IsDebuggingEnabledKey = nameof(GameManager) + "." + nameof(IsDebuggingEnabled);
        private static bool isDebuggingEnabled;

        private bool isInitialActorStartupCompleted;

        private ISerializer serializer;
        private IStorage runtimeStorage;
        private IResourceLoader resourceLoader;
        private IActorManager actorManager;
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

#if UNITY_EDITOR
            if (TryReadEditorData(IsDebuggingEnabledKey, out bool value))
            {
                isDebuggingEnabled = value;
            }
#else
            isDebuggingEnabled = Debug.isDebugBuild;
#endif

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
            var context = new PhysicsUpdateContext(deltaTime: Time.deltaTime, time: Time.time);
            var actors = actorManager.Actors;

            for (var index = actors.Count - 1; index >= 0; index--)
            {
                var actor = actors[index];

                try
                {
                    actor.UpdatePhysics(context);
                }
                catch (Exception exception)
                {
                    Logging.LogException(exception, this);
                }
            }
        }

        private void Update()
        {
            var context = new FrameUpdateContext(deltaTime: Time.deltaTime, time: Time.time);
            var actors = actorManager.Actors;

            for (var index = actors.Count - 1; index >= 0; index--)
            {
                var actor = actors[index];

                try
                {
                    actor.UpdateFrame(context);
                }
                catch (Exception exception)
                {
                    Logging.LogException(exception, this);
                }
            }
        }

        private void OnDestroy()
        {
            OnDisposeEntered();

            if (actorManager != null)
            {
                var actors = actorManager.Actors;
                foreach (var actor in actors)
                {
                    RemoveActor(actor);
                }
            }

            var profile = Settings.ActiveProfile;
            if (profile.IsVerboseLogging)
            {
                Logging.LogDebug($"{GetGameManagerName()} disposed", this);
            }

            OnDisposedExited();
        }

        /// <summary>
        /// Called when actors are about to initialize and should be added to the game.
        /// </summary>
        protected virtual void OnInitializeActorsEntered()
        {
        }

        /// <summary>
        /// Called when all actors are initialized.
        /// </summary>
        protected virtual void OnInitializeActorsExited()
        {
        }

        /// <summary>
        /// Called the game manager is about to be disposed.
        /// </summary>
        protected virtual void OnDisposeEntered()
        {
        }

        /// <summary>
        /// Called the game manager is destroyed.
        /// </summary>
        protected virtual void OnDisposedExited()
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
        /// Add given <paramref name="actor"/> to the <see cref="actorManager"/>.
        /// </summary>
        protected void AddActor(IActor actor)
        {
            if (actorManager.AddActor(actor) == false)
            {
                return;
            }

            if (isInitialActorStartupCompleted)
            {
                // We automatically initialize after all initial actors are initialized only. This way we can finely
                // control the init order on startup.
                actor.Initialize();
            }

            Publish(new ActorAddedMessage(actor));
        }

        /// <summary>
        /// Remove <paramref name="actor"/> from the <see cref="actorManager"/>.
        /// </summary>
        protected void RemoveActor(IActor actor)
        {
            if (actorManager.RemoveActor(actor) == false)
            {
                return;
            }

            actor.Dispose();

            Publish(new ActorRemovedMessage(actor));
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
            return new DefaultFileStorage(
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
            return new DefaultResourceLoader(serializer);
        }

        /// <returns>
        /// New <see cref="IActorManager"/> instance.
        /// </returns>
        protected virtual IActorManager CreateActorManager()
        {
            var profile = Settings.ActiveProfile;
            return new DefaultActorManager(profile);
        }

        /// <returns>
        /// New <see cref="IMessageBus"/> instance.
        /// </returns>
        protected virtual IMessageBus CreateMessageBus()
        {
            return new DefaultMessageBus();
        }

        private void InitializeGameManager()
        {
            InitializeCore();

            OnInitializeActorsEntered();
            InitializeActors();
            OnInitializeActorsExited();
        }

        private void InitializeCore()
        {
            name = GetGameManagerName();
            serializer = CreateSerializer();
            runtimeStorage = CreateRuntimeStorage();
            resourceLoader = CreateResourceLoader();
            actorManager = CreateActorManager();
            messageBus = CreateMessageBus();
        }

        private void InitializeActors()
        {
            var profile = Settings.ActiveProfile;
            var actors = actorManager.Actors;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < actors.Count; index++)
            {
                var actor = actors[index];

                if (profile.IsVerboseLogging)
                {
                    var actorType = actor.GetType();
                    var actorName = actorType.Name;

                    Logging.LogDebug($"Initializing actor {actorName}", this);
                }

                actor.Initialize();
            }

            isInitialActorStartupCompleted = true;
        }

        private static GameManager GetGameManager()
        {
#if UNITY_EDITOR
            if (GameManagerUtilities.IsApplicationQuitting)
            {
                // We don't care about warnings if the application is quitting.
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
    }
}
