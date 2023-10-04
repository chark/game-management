# Game Management

[![Unity 2022.3+](https://img.shields.io/badge/unity-2022.3%2B-blue.svg)](https://unity3d.com/get-unity/download)
[![Actions Status](https://github.com/chark/game-management/workflows/CI/badge.svg)](https://github.com/chark/game-management/actions)

Essential and minimalistic game management tools.

<p align="center">
  <img src="screenshot.png"/>
</p>

:warning: **Warning, this is a preview package, expect breaking changes between releases!**

## Features

- Service Locator
- Message Bus
- Data storage
- Automatic initialization

## Installation

This package can be installed via the Package Manager by [Installing from a Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html):

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
- [License](../Packages/com.chark.game-management/LICENSE.md)