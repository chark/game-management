[Unity Package Manager]: https://docs.unity3d.com/Manual/upm-ui.html
[Changelog]: ../CHANGELOG.md

[Samples~]: ../Samples%7E
[Defaults Sample]: ../Samples%7E/Defaults

[Game Manager]: ../Runtime/GameManager.cs
[Game Manager Settings]: ../Runtime/GameManagerSettings.cs
[Game Manager Settings Profile]: ../Runtime/Settings/GameManagerSettingsProfile.cs

[Default Game Manager]: ../Samples%7E/Defaults/Scripts/DefaultGameManager.cs
[Default Game Manager Prefab]: ../Samples%7E/Defaults/Prefabs/DefaultGameManager.prefab
[Default Game Manager Settings Profile]: ../Samples%7E/Defaults/Resources/DefaultGameManagerSettingsProfile.asset

[SimpleActor]: ../Runtime/Actors/SimpleActor.cs
[MonoActor]: ../Runtime/Actors/MonoActor.cs
[SimpleSystem]: ../Runtime/Actors/SimpleSystem.cs
[MonoSystem]: ../Runtime/Actors/MonoSystem.cs
[IMessage]: ../Runtime/Messaging/IMessage.cs

[IFixedUpdateListener]: ../Runtime/Systems/IFixedUpdateListener.cs
[IUpdateListener]: ../Runtime/Systems/IUpdateListener.cs

# Documentation

This package provides a set of runtime scripts for use as a backbone for your game. It is code-first, so you'll need quite a lot of C# code to use this package.

Make sure to keep any eye for breaking changes in [Changelog].

## Getting Started

Open [Unity Package Manager], select _Game Management_ package and import [Defaults Sample]:

<p align="center">
  <img src="samples.png"/>
</p>

This will import:

- [Default Game Manager] - script which can be used as a template for your custom game manager.
- [Default Game Manager Prefab] - prefab which can be used as a template for your game manager prefab.
- [Default Game Manager Settings Profile] - asset which defines how the [Default Game Manager] should load.

After importing, open the [Game Manager Settings] asset (located at `Assets/Settings/GameManagerSettings.asset` by default) and add [Default Game Manager Settings Profile] to the **Profiles** field. Also make sure this profile is active:

<p align="center">
  <img hspace="2%" src="game-manager-settings.png"/>
</p>

Run the game!

## Game Manager Settings

[Game Manager Settings] asset will be created automatically for you when you install this package. The default location is at `Assets/Settings/GameManagerSettings.asset` but you can move it anywhere in your project.

[Game Manager Settings] asset defines which [Game Manager] should load and how. If there are more than one profiles added to this asset, the first profile which is active will be used. If no profiles are active or the list is empty, a no-op default profile will be utilized.

When you create this asset for the first time, it will have no **Profiles** added. To override the defaults you'll need to define a custom [Game Manager Settings Profile] asset, configure it and add to this list. You can create [Game Manager Settings Profile] assets the following way:

- Right-click anywhere in the _Project Window_ and select _Create/CHARK/Game Management/Game Manager Settings Profile_.
- Add it to the [Game Manager Settings] asset **Profiles** list.

<p align="center">
  <img hspace="2%" src="game-manager-settings-profile.png"/>
</p>

Available properties for customization on [Game Manager Settings Profile]:

- **Is Active Profile** - useful when you have multiple [Game Manager Settings Profile] assets. You can use this field to define which settings asset should be used.
- **Is Instantiate Automatically** - should [Game Manager] be instantiated automatically when the game starts. Disable this if you want to manage the lifecycle manually.
- **Instantiation Mode** - when to load the [Game Manager], for more info see [InitializeOnLoadAttribute](https://docs.unity3d.com/ScriptReference/InitializeOnLoadAttribute.html).
- **Game Manager Prefab** - [Game Manager] prefab to instantiate when the game starts, used when **Is Instantiate Automatically** is set to `true`.

## Scripting

[Game Manager] provides a set of static methods to interact with its functionality. Additionally, it is fully customizable, meaning you can override how it sends messages by supplying custom implementations in of various sub-systems.

### Game Manager

The starting point when using this package should be a class that inherits [Game Manager]. Here you can define your systems, initialize game state and override default behavior:

```csharp
using CHARK.GameManagement;
using CHARK.GameManagement.Assets;
using CHARK.GameManagement.Actors;
using CHARK.GameManagement.Messaging;
using CHARK.GameManagement.Storage;

internal sealed class MyGameManager : GameManager
{
    protected override void OnInitializeActorsEntered()
    {
        // Initialize actors here
        // AddActor(...);
    }

    protected override void OnInitializeActorsExited()
    {
        // Do stuff with actors here after they're initialized and handle general game init logic
    }

    protected override void OnDestroyEntered()
    {
        // Cleanup before game manager destroy
    }
  
    protected override void OnDestroyExited()
    {
        // Cleanup after game manager destroy
    }

    protected override string GetGameManagerName()
    {
        return "My Game Manager";
    }
    
    protected override IGameStorage CreateRuntimeGameStorage()
    {
        // Provide a custom storage implementation
    }

    protected override IResourceLoader CreateResourceLoader()
    {
        // Provide a custom resource loading implementation
    }

    protected override IActorManager CreateActorManager()
    {
        // Provide a custom actor storage implementation
    }

    protected override IMessageBus CreateMessageBus()
    {
        // Provide a custom messaging implementation
    }
}
```

Some things to keep in mind:

- All of the `override` methods are optional.
- It is safe to retrieve actors in `OnInitializeActorsExited` method.
- `OnInitialized` method of each actor is called in the order the actor was registered.

### Game Manager Static Methods

To communicate with the currently active management backend in your regular components and gameplay code, use static methods exposed on [Game Manager] class:

```csharp
// Subscribe to messages
GameManager.AddListener<MyMessage>(message => { });

// Unsubscribe from messages
GameManager.RemoveListener<MyMessage>(message => { });

// Publish a message
GameManager.Publish(new MyMessage());

// Retrieve a set of actors
var systems = GameManager.GetActors<IMyActor>();

// Retrieve actor or throw an exception if its missing
var system = GameManager.GetActor<IMyActor>();

// Retrieve a actor
if (GameManager.TryGetActor<IMyActor>(out var actor))
{
}

// Register an actor
if (GameManager.AddActor(actor))
{
  // Actor registered
}

// Unregister an actor
if (GameManager.RemoveActor(actor))
{
  // Actor removed
}

// Listen for added/removed actors
GameManager.AddListener<ActorAddedMessage>(message => { });
GameManager.AddListener<ActorRemovedMessage>(message => { });

// Load a set of resources
var resources = GameManager.LoadResources<MyResource>();

// Load a single resource
var resource = GameManager.LoadResource<MyResource>();

// Load a value asynchronously
var value = await GameManager.GetRuntimeValueAsync<MyValue>("some-key");

// Load a value synchronously
if (GameManager.TryGetRuntimeValue<MyValue>("some-key", out var value))
{
}

// Save a value asynchronously
await GameManager.SetRuntimeValueAsync("some-key", value);

// Save a value synchronously
GameManager.SetRuntimeValue("some-key", value);

// Delete a value asynchronously
await GameManager.DeleteRuntimeValueAsync("some-key");

// Delete a value synchronously
GameManager.DeleteRuntimeValue("some-key");

// Load an Editor value synchronously
if (GameManager.TryGetEditorValue<MyValue>("some-key", out var value))
{
}

// Save an Editor value synchronously
GameManager.SetEditorValue("some-key", value);

// Delete an Editor value synchronously
GameManager.DeleteEditorValue("some-key");

// Change debugging mode state
GameManager.IsDebuggingEnabled = true;
GameManager.IsDebuggingEnabled = false;

// Check for debug mode state changes and react accordingly
GameManager.AddListener<DebuggingChangedMessage>(message => { });
```

### Actors/Systems

Actors are your "root" components, which hide a set of regular Unity components. The idea behind actors is to provide a clean interface for them to interact with other actors and the game world. Essentially, if anything is interactable, it should be an actor. Additionally, it is recommended to interact with the `GameManager` in actors exclusively if you want to keep things tidy.

Systems are essentially the same as actors and are only separated for now to make it easier to differentiate from regular actors. Essentially, systems are actors which act as back-end game systems (e.g., database, music).

When creating custom actors or systems, you can inherit from the following classes:

- [SimpleActor] - actor which is a plain old C# class.
- [MonoActor] - actor which inherits a [MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html) which will enable you to use [SerializeField](https://docs.unity3d.com/ScriptReference/SerializeField.html) and other Unity serialization goodies.
- [SimpleSystem] - system which inherits [SimpleActor].
- [MonoSystem] - system which inherits [MonoActor].

Here are some examples of gameplay actors:

```csharp
// It is recommended to create interfaces for all of your actors to better define the API
interface IPlayerActor : IActor
{
}

// Mono* classes will add to GameManager automatically
internal sealed class MonoPlayerActor : MonoActor, IPlayerActor
{
    [SerializeField]
    private int hp = 100;

    protected override void OnInitialized()
    {
    }

    protected override void OnDisposed()
    {
    }

    protected override void OnUpdatedPhysics(IUpdateContext context)
    {
    }

    protected override void OnUpdatedFrame(IUpdateContext context)
    {
    }
}

// You'll need to add this to the GameManager manually via GameManager.AddActor(new SimplePlayerActor(123))
internal sealed class SimplePlayerActor : SimpleActor, IPlayerActor
{
    private readonly int hp;

    public SimplePlayerActor(int hp)
    {
        this.hp = hp;
    }

    protected override void OnInitialized()
    {
    }

    protected override void OnDisposed()
    {
    }

    protected override void OnUpdatedPhysics(IUpdateContext context)
    {
    }
  
    protected override void OnUpdatedFrame(IUpdateContext context)
    {
    }
}
```

And here are some examples of system actors:

```csharp
// It is recommended to create interfaces for systems as well
interface IDataSystem : IActor
{
}

// Mono* classes will add to GameManager automatically
internal sealed class MonoDataSystem : MonoSystem, IDataSystem
{
    [SerializeField]
    private string dbUrl = "sqlite.something";

    protected override void OnInitialized()
    {
    }

    protected override void OnDisposed()
    {
    }

    protected override void OnUpdatedPhysics(IUpdateContext context)
    {
    }
  
    protected override void OnUpdatedFrame(IUpdateContext context)
    {
    }
}

// You'll need to add this to the GameManager manually via GameManager.AddActor(new SimpleDataSystem("..."))
internal sealed class SimpleDataSystem : SimpleSystem, IDataSystem
{
    private readonly string dbUrl;

    public SimpleDataSystem(int dbUrl)
    {
        this.dbUrl = dbUrl;
    }

    protected override void OnInitialized()
    {
    }

    protected override void OnDisposed()
    {
    }
  
    protected override void OnUpdatedPhysics(IUpdateContext context)
    {
    }
  
    protected override void OnUpdatedFrame(IUpdateContext context)
    {
    }
}
```

### Messaging

To create and send custom messages, define a class which inherits [IMessage] and you're ready to go:

```csharp
internal readonly struct MyMessage : IMessage
{
}
```

Afterward, you can fire away your messages like so:

```csharp
GameManager.Publish(new MyMessage());
```