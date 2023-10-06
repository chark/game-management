# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project
adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [v0.0.2](https://github.com/chark/game-management/compare/v0.0.1...v0.0.2) - 2023-10-06

### Added

- `GameManagerSettingsProfile` which can be used to load a specific `GameManager` configuration via `GameManagerSettings`.

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
