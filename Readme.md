## SRL API Client

This repository contains a basic .Net client library for the [SpeedRunsLive](http://speedrunslive.com) API. The client is available via [NuGet](https://www.nuget.org/packages/SRLApiClient).

```bash
#.Net
dotnet add package SRLApiClient

#Package Manager
Install-Package SRLApiClient
```

---

### Requirements

.Net Core 2.0 or greater. Platform specific prequisites can be found here:
- [Windows](https://docs.microsoft.com/en-us/dotnet/core/windows-prerequisites?tabs=netcore21),
- [Linux](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites?tabs=netcore2x),
- [macOS](https://docs.microsoft.com/en-us/dotnet/core/macos-prerequisites?tabs=netcore2x)

---

### Usage

#### Initialization

##### `SRLClient`

To create a client you can simply use the following statement:

```c#
SRLClient Client = new SRLClient();
```

To use the Client with a custom host that uses the SRL flow you can pass the host as parameter. The default host is `"speedrunslive.com"`

```c#
SRLClient Client = new SRLClient("example.com");
```

---

#### Authentication

Certain requests require authentication. To authenticate the client you're gonna need valid SRL credentials.

```c#
Client.Authenticate("psychonauter", "password");
```

`Authenticate()` returns a boolean value that tells you if the authentication was successful.


---

#### Endpoints


##### Countries

Use the following to get a list of available countries on SRL:

```
ReadOnlyCollection countries = Client.Countries.Get();
```

---

##### Games

To fetch a game from SRL you need to know its abbrevation.

```c#
Game game = Client.Games.Get("sms");
```

or simply

```c#
Game game = Client.Games["sms"];
```

You can also search for games to find the correct abbrevation.

```c#
ReadOnlyCollection<Game> searchResults = Client.Games.Search("super mario sunshine");
```

---

##### Leaderboards

To fetch a leaderboard for a game from SRL you need to know the games abbrevation.

```c#
Leaderboard board = Client.Leaderboards.Get("sms");
```

or

```c#
Leaderboard board = Client.Leaderbaords["sms"];
```

---

##### Races

Use the following to get a list of active races:

```c#
ReadOnlyCollection<Race> activeRaces = Client.Races.GetActive();
```

To fetch a specific Race you need to know the race id:

```
Race race = Client.Races.Get("someId");
```

or

```c#
Race race = Client.Races["someId"];
```

Creating a race requires **authentication**. Also, you either have to be racebot or you need at least the 'voice' role on SRL. If you don't know that this is you most probably don't have it.

```c#
Race newRace = Client.Races.Create("sms");
```

---

##### PastRaces

PastRaces work the same as Races, except that you can't create new ones.

---

##### Players

To get a player you need to know their username.

```c#
Player player = Client.Players.Get("psychonauter");
```

or

```c#
Player player = Client.Players["psychonauter"];
```

You can also search for players using:

```c#
ReadOnlyCollection<Player> players = Client.Players.Search("psychonauter");
```

You can edit your own profile if you are authenticated.

```c#
Client.User.SetTwitter("psychonauter");
```

or

```c#
Client.Players.Edit("psychonauter", twitter: "psychonauter");
```

---

