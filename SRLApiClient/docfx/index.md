# SRLApiClient
A .NET Standard 2.0 wrapper around the [SpeedRunsLive](http://speedrunslive.com) API.


## Quick Start Notes:

### Installation

You can install the client via [NuGet](https://www.nuget.org/packages/SRLApiClient):


```bash
#.Net
dotnet add package SRLApiClient

#Package Manager
Install-Package SRLApiClient
```

### Requirements

The SRL API Client supports any platform that implements [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard#net-implementation-support).

### Dependencies

_none_

## Changelog

#### v1.1.1

- Ensured proper disposal of semaphores in the HTTP client pool

#### v1.1.0

- Less lenient exception handling - if the client fails to parse a response, the `SRLParseException` is no longer caught automatically
- Added `GetAll()` as bulk request to the `Games` endpoint

#### v1.0.0

- Initial Release

#### v1.0.0-rc3

- Changed build runtime to .NET Core SDK 2.2.106

#### v1.0.0-rc2

- Fixex Timeout not updating when changed

#### v1.0.0-rc1

- Added an HTTP client pool to run multiple requests in parallel

#### v0.10.0-beta2

- Added paginated requests for past races

#### v0.10.0-beta

- Added asynchronous methods

#### v0.9.1

- Fixed spelling of 'abbreviation'

#### v0.9.0

- Fixed partially disfunctional extension methods