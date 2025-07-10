# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project
adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [v0.0.3](https://github.com/chark/game-management/compare/v0.0.2...v0.0.3) - 2023-XX-XX

### Added

- `GameManager.TryDeserializeValue`, `GameManager.TrySerializeValue`, `GameManager.TryDeserializeStream` and `GameManager.TrySerializeStream` methods, these can be used to serialize and deserialize data using internal `GameManager` serialization utilities. You can customize this via `ISerializer` interface (see `CreateSerializer` method on `GameManager`).
- `GameManager.DeleteDataAsync` which can be used to delete data asynchronously.
- `GameManager.ReadResourceAsync` and `GameManager.ReadResourceStreamAsync` methods which can be used to retrieve resources from StreamingAssets directory.
- `GameManager.ReadDataStream` and `GameManager.ReadDataStreamAsync` methods which can be used to read a `Stream` from a file on disk.
- `GameManager.SaveDataStream` and `GameManager.SaveDataStreamAsync` methods which can be used to persist a `Stream` to disk.
- `GameManager.IsDebuggingEnabled` flag which can be used to turn on if debug mode is on for the game manager.
- `GameManager.IsApplicationQuitting` flag which can be used to check if app is quitting (works in Editor as well).
- `DebuggingChangedMessage` which can be used to check if `GameManager.IsDebuggingEnabled` changes.
- `ApplicationQuittingMessage` which can be used to detect if application is quitting (works in Editor as well).
- Caching to `DefaultEntityManager`.

### Changed

- Renamed some `IResourceLoader` methods to use `Get*` prefix instead of `Load*` so its more consistent with other methods. Methods which read from _StreamingAssets_ directory will use `Read*` prefix.
- Renamed `IGameStorage` to `IStorage`.
- Cancellation tokens can now be used in all async methods.
- Renamed `IStorage` methods to use `Read*` and `Save*` prefixes to emphasise that these methods interact with data on dist.
- Project directory name will be used for editor storage keys, this should prevent editor key conflict issues with Parallel sync and similar plugins.

### Fixed

- `GameStorage.GetValueAsync` (now `GameStorage.ReadValueAsync`) not switching back to main thread when no value is found.
- Message bus not handling abstract and interface listeners.
- Message bus iteration breaking when listener would remove itself during `Raise` call.

## [v0.0.2](https://github.com/chark/game-management/compare/v0.0.1...v0.0.2) - 2023-10-06

### Added

- `GameManagerSettingsProfile` which can be used to load a specific `GameManager` configuration via `GameManagerSettings`.
- Better logging with a custom log message format.
- Property Drawer for `GameManagerSettingsProfile`, this way it's easier to manage lists of profiles.

### Changed

- `GameManagerSettings` will now be automatically created at `Assets/Settings/GameManagerSettings.asset` and added to preloaded asset list (you can move it later). This is needed to support automatic instantiation as previously using `Resources.FindObjectsOfTypeAll` was not working in builds.
- `GameManagerSettings` class visibility is now `private`.
- `GameManager` static methods will now throw an `Exception` if it's not initialized instead of just logging an error and pausing.
- Automatic `GameManager` instantiation now only supports loading **before** and **after** scene load.
- Menu item order to use `150` instead of `-1000`. This way `CHARK` won't dominate existing entries.

### Removed

- `GameManager.GetGameManagerSettings` method as a new profile system will be used instead. Overriding this doesn't make sense anymore.

## [v0.0.1](https://github.com/chark/game-management/compare/v0.0.1) - 2023-10-04

Initial preview version.

### Added

- Core Game Management logic.
- System adding, removal and retrieval.
- Messaging solution.
- Storage solution with built-in json support.
- Documentation.
