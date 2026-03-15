# AGENTS.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Inflatable Last.fm** — a .NET SDK for the Last.fm REST API. Targets `netstandard1.1` for broad platform compatibility (.NET Core, .NET Framework 4.5.1+, UWP, Xamarin). ~79% of the Last.fm API is implemented; see `PROGRESS.md` for coverage details.

## Commands

```bash
# Restore dependencies
dotnet restore IF.Lastfm.sln

# Build
dotnet build -c Release --no-restore IF.Lastfm.sln

# Test
dotnet test --no-restore IF.Lastfm.sln

# Run a single test project
dotnet test src/IF.Lastfm.Core.Tests/IF.Lastfm.Core.Tests.csproj

# Pack NuGet package
dotnet pack -c Release --no-build --include-source -o ./tmp IF.Lastfm.sln
```

CI uses .NET 6.0. The `release` branch triggers publishing to nuget.org.

## Architecture

### Command Pattern

Every Last.fm API method is implemented as a **command class** in `src/IF.Lastfm.Core/Api/Commands/{Module}/`. Commands inherit from:
- `GetAsyncCommandBase<T>` — for GET requests
- `PostAsyncCommandBase<T>` — for POST requests (auth, scrobbling)

When adding a new API method, create a command class in the appropriate module subdirectory, then expose it through the corresponding `*Api.cs` class.

### Entry Point: `LastfmClient`

`src/IF.Lastfm.Core/Api/LastfmClient.cs` is the public facade. It lazily initializes API modules (`Album`, `Artist`, `Track`, `User`, etc.) and holds the shared `LastAuth` and `HttpClient`.

```
LastfmClient
  ├── Auth (LastAuth)          — session management
  ├── Album (AlbumApi)         → Commands/Album/
  ├── Artist (ArtistApi)       → Commands/Artist/
  ├── Track (TrackApi)         → Commands/Track/
  ├── User (UserApi)           → Commands/User/
  └── ...
```

`ApiBase` (in `Helpers/`) is the shared base holding `HttpClient`, `Auth`, and `Dispose` logic.

### Response Types

- `LastResponse` / `LastResponse<T>` — single-item responses
- `PageResponse<T>` — paginated responses

JSON deserialization uses Newtonsoft.Json via contract types in `src/IF.Lastfm.Core/Json/`.

### Scrobblers

`src/IF.Lastfm.Core/Scrobblers/` — `MemoryScrobbler` is the default. Custom scrobblers extend `ScrobblerBase`. The SQLite persistence layer lives in `src/IF.Lastfm.SQLite/`.

### Testing

- **Unit tests** (`IF.Lastfm.Core.Tests`): NUnit + Moq. Tests mock the `HttpClient` and feed stored JSON responses from `Resources/`.
- **Integration tests** (`IF.Lastfm.Core.Tests.Integration`): Hit the real Last.fm API.

When writing tests, use embedded JSON fixtures (real API responses) rather than constructing objects manually. Follow the pattern in existing test classes for the module you are working on.

### Syro Developer Tool

`src/IF.Lastfm.Syro/` is a WPF GUI app (.NET 4.6.2) used to interactively test API methods and generate `PROGRESS.md`. Not part of the main solution (`IF.Lastfm.sln`) — use `IF.Lastfm.Testing.sln` to include it.
