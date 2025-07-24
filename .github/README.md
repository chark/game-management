# Game Management

[![Unity 6000.0+](https://img.shields.io/badge/unity-6000.0%2B-blue.svg)](https://unity3d.com/get-unity/download)
[![openupm](https://img.shields.io/npm/v/com.chark.game-management?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.chark.game-management/)
[![Actions Status](https://github.com/chark/game-management/workflows/CI/badge.svg)](https://github.com/chark/game-management/actions)

:warning: **Warning, this is a preview package, expect breaking changes between releases!**

Essential, minimalistic and code-first Game Management tools. Perfect for game-jams and medium-sized projects.

```csharp
class MyGameManager : GameManager
{
    protected override void OnBeforeInitializeSystems()
    {
        AddSystem(new PlayerSystem());
    }
}

class Player : MonoBehavior
{
    PlayerSystem playerSystem;

    void Awake() 
    {
        playerSystem = GameManager.GetSystem<PlayerSystem>();
    }
    
    void Start()
    {
        GameManager.Publish(new PlayerSpawnedMessage());
    }
}
```

## Features

- Code-first
- Service Locator
- Message Bus
- JSON Serialization/deserialization utilities
- File reading/writing with JSON support
- Logging utilities
- Automatic initialization
- Odin Inspector support
- UniTask support

## Installation

This package can be installed via [OpenUPM](https://openupm.com/packages/com.chark.game-management):
```text
openupm add com.chark.game-management
```

Or via the Unity Package Manager by [Installing from a Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html):

```text
https://github.com/chark/game-management.git#upm
```

Alternatively, manually install by adding the following entry to `Packages/manifest.json`:
```json
{
  "com.chark.game-management": "https://github.com/chark/game-management.git#upm"
}
```

If you'd like to install a specific release, replace `upm` suffix with version number, e.g., `v0.0.1`. You can find all releases [here](https://github.com/chark/game-management/releases).

## Links

- [Documentation](../Packages/com.chark.game-management/Documentation~/README.md)
- [Contributing](CONTRIBUTING.md)
- [Changelog](../Packages/com.chark.game-management/CHANGELOG.md)
