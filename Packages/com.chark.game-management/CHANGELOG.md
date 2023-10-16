# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project
adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [v0.0.3](https://github.com/chark/game-management/compare/v0.0.2...v0.0.3) - 2023-XX-XX

### Added

- `ISerializer` with `GameManager.TryDeserializeValue` and `GameManager.TrySerializeValue` methods, this can be used to serialize and deserialize data using internal `GameManager` serialization utilities.
- `GameManager.DeleteRuntimeValueAsync` which can be used to delete data asynchronously.
- `GameManager.GetResourceAsync` which can be used to retrieve resources from StreamingAssets directory.

### Changed

- Renamed `IResourceLoader` methods to use `Get*` prefix instead of `Load*` so its more consistent with other methods.
- Renamed `IGameStorage` to `IStorage`.
- Cancellation tokens can now be used in async methods.

### Fixed

- `GameStorage.GetValueAsync` not switching back to main thread when no value is found.

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
