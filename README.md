# Lastream Last.fm .NET SDK

[![Code licence](https://img.shields.io/badge/licence-MIT-blue.svg?style=flat)](LICENCE.md)
[![NuGet](https://img.shields.io/nuget/v/Lastream.Lastfm.svg)](https://www.nuget.org/packages/Lastream.Lastfm/)

> This is a maintained fork of [inflatablefriends/lastfm](https://github.com/inflatablefriends/lastfm) (originally published as `Inflatable.Lastfm`), which has not seen active development since 2019. The fork preserves full backwards compatibility while adding improvements described below.

## What this fork adds

- **Targets .NET 10** — dropped `netstandard1.1` in favour of `net10.0`; `System.Net.Http` is no longer a separate dependency

- **Richer error details on all responses** — `LastResponse` now exposes:
  - `ErrorMessage` — the human-readable message from the Last.fm API (e.g. `"The artist you supplied could not be found"`)
  - `HttpStatusCode` — the HTTP status code returned by the server
  - `Exception` — the `HttpRequestException` when a network failure occurs (`Status == RequestFailed`)

- **Web authentication flow** (`auth.getSession`) — `client.Auth.GetSessionTokenAsync(authToken)` exchanges a web-auth callback token for a session key, enabling the [Last.fm web auth flow](https://www.last.fm/api/webauth)

- **Desktop authentication flow** (`auth.getToken`) — `client.Auth.GetAuthTokenAsync()` fetches an unauthorised request token as step 1 of the [desktop auth flow](https://www.last.fm/api/desktopauth)

- **Artist-filtered track search** — `client.Track.SearchAsync` accepts an optional `artistName` parameter to narrow results to a specific artist

- **Removed `LastArtist.MainImage`** — the property was marked obsolete since May 2019; Last.fm has not returned real artist imagery via the API since then (all responses contain the same generic placeholder image)

## Project Goals

- To provide complete .NET bindings for the Last.fm REST API
- To build useful components for Last.fm applications

## Contributing

[Raise an issue on GitHub](https://github.com/kapral/lastfm/issues) or submit a pull request.

If you're interested in contributing code or documentation, [this short introduction to the library](doc/contributing.md) will help you get started.

## Quickstart

### Installing

#### NuGet

Install [Lastream.Lastfm](https://www.nuget.org/packages/Lastream.Lastfm/) from NuGet.

#### From source

1. Install the [.NET SDK](https://docs.microsoft.com/en-us/dotnet/core/install/sdk)
2. Clone this repo
3. Run `dotnet pack`
4. Reference the built NuGet package file in your project

### Examples

First, [sign up for Last.fm API](http://last.fm/api) access if you haven't already.

Create a LastfmClient:

```c#
var client = new LastfmClient("apikey", "apisecret");
```

Get information about an album:

```c#
var response = await client.Album.GetInfoAsync("Grimes", "Visions");

LastAlbum visions = response.Content;
```

For methods that return several items, you can iterate over the response:

```c#
var pageResponse = await client.Artist.GetTopTracksAsync("Ben Frost", page: 5, itemsPerPage: 100);

var trackNames = pageResponse.Select(track => track.Name);
```

Several API methods require user authentication. Once you have your user's Last.fm username and password, you can authenticate your instance of LastfmClient:

```c#
var response = await client.Auth.GetSessionTokenAsync("username", "pass");

// or load an existing session
UserSession cachedSession;
var successful = client.Auth.LoadSession(cachedSession);
```

Authenticated methods then work like any other:

```c#
if (client.Auth.HasAuthenticated) {
    var response = await client.Track.LoveAsync("Ibi Dreams of Pavement (A Better Day)", "Broken Social Scene");
}
```

### Error handling

All responses surface full error details:

```c#
var response = await client.Artist.GetInfoAsync("nonexistent artist");

if (!response.Success) {
    Console.WriteLine(response.Status);        // LastResponseStatus.MissingParameters
    Console.WriteLine(response.ErrorMessage);  // "The artist you supplied could not be found"
    Console.WriteLine(response.HttpStatusCode); // System.Net.HttpStatusCode.OK
    Console.WriteLine(response.Exception);     // non-null only for network failures
}
```

## Documentation

- [API method progress report](PROGRESS.md)
- [Contributing](doc/contributing.md)
- [Scrobbling](doc/scrobbling.md)
- [Dependency Injection](doc/dependency-injection.md)

## Platform Compatibility

The main package targets `net10.0`.

### Dependencies

- Newtonsoft.Json 13.0.3

## Credits

Original library by [@rikkilt](http://twitter.com/rikkilt) and [contributors](https://github.com/inflatablefriends/lastfm/graphs/contributors).
Fork maintained by [@kapral](https://github.com/kapral).
