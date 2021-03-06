﻿# SRL API Client

[![NuGet](https://img.shields.io/nuget/v/SRLApiClient.svg)](https://www.nuget.org/packages/SRLApiClient) [![NuGet Pre-Release](https://img.shields.io/nuget/vpre/SRLApiClient.svg?label=nuget%20pre-release)](https://www.nuget.org/packages/SRLApiClient) [![Nuget Downloads](https://img.shields.io/nuget/dt/SRLApiClient.svg)](https://www.nuget.org/packages/SRLApiClient) [![Build Status](https://dev.azure.com/bitpatty/SRLApiClient/_apis/build/status/BitPatty.SRLApiClient?branchName=master)](https://dev.azure.com/bitpatty/SRLApiClient/_build/latest?definitionId=2&branchName=master) [![GitHub license](https://img.shields.io/badge/license-AGPLv3-blue.svg)](https://raw.githubusercontent.com/BitPatty/SRLApiClient/master/LICENSE)

This repository contains a compact, asynchronous .Net Standard 2.1 wrapper around the [SpeedRunsLive](http://speedrunslive.com) API. The client is available via [NuGet](https://www.nuget.org/packages/SRLApiClient).

```bash
#.Net
dotnet add package SRLApiClient

#Package Manager
Install-Package SRLApiClient
```

---

## Features

The client supports synchronous and asynchronous GET requests on the following endpoints:
- Countries
- Games
- Leaderboards
- Races
- Past Races
- Players
- User

Authentication via HTTP is supported to update user profiles. POST/PUT requests on other endpoints are not possible, since users are missing the required privileges.

---

## Requirements

The SRL API Client supports any platform that implements [.NET Standard 2.1](https://docs.microsoft.com/en-us/dotnet/standard/net-standard#net-implementation-support).

### Dependencies

_none_

---

## Usage

You can visit [srlclient.zint.ch](https://srlclient.zint.ch) for the autogenerated API documentation.

---

## License

```
SRLApiClient - A .NET Standard wrapper around the SpeedRunsLive API
Copyright (C) 2018-2020  Matteias Collet

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
```
