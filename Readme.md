# SRL API Client

This repository contains a basic .Net client library for the [SpeedRunsLive](http://speedrunslive.com) API. The client is available via [NuGet](https://www.nuget.org/packages/SRLApiClient).

```bash
#.Net
dotnet add package SRLApiClient

#Package Manager
Install-Package SRLApiClient
```

---

## Requirements

.Net Core 2.0 or greater. Platform specific prequisites can be found here:

- [Windows](https://docs.microsoft.com/en-us/dotnet/core/windows-prerequisites?tabs=netcore21),
- [Linux](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites?tabs=netcore2x),
- [macOS](https://docs.microsoft.com/en-us/dotnet/core/macos-prerequisites?tabs=netcore2x)

---

## Usage

### Initialization

#### `SRLClient`

To create a client you can simply use the following statement:

```c#
SRLClient Client = new SRLClient();
```

To use the Client with a custom host that uses the SRL flow you can pass the host as parameter. The default host is `"speedrunslive.com"`

```c#
SRLClient Client = new SRLClient("example.com");
```

##### Properties

| Property     | Type                                        | Description                               |
| ------------ | ------------------------------------------- | ----------------------------------------- |
| Games        | `Endpoints.Games.GamesClient`               | Client for fetching games                 |
| Players      | `Endpoints.Players.PlayersClient`           | Client for fetching player data           |
| Leaderboards | `Endpoints.Leaderboards.LeaderboardsClient` | Client for fetching leaderboard data      |
| Races        | `Endpoints.Races.RacesClient`               | Client for fetching races                 |
| PastRaces    | `Endpoints.PastRaces.PastRacesClient`       | Client for fetching past races            |
| Countries    | `Endpoints.Countries.CountriesClient`       | Client for fetching countries             |
| Stats        | `Endpoints.Stats.StatsClient`               | Client for fetching game and player stats |

---

### Authentication

Certain requests require authentication. To authenticate the client you're gonna need valid SRL credentials.

```c#
Client.Authenticate("psychonauter", "password");
```

`Authenticate()` returns a boolean value that tells you if the authentication was successful.

---

### Endpoints

#### Countries

Use the following to get a list of available countries on SRL:

```
ReadOnlyCollection<string> countries = Client.Countries.Get();
```

##### Properties

_none_

---

#### Games

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

##### Properties

| Property       | Type     | Description                |
| -------------- | -------- | -------------------------- |
| Id             | `int`    | Game Id                    |
| Name           | `string` | Full name of the game      |
| Abbrevation    | `string` | The games abbrevation      |
| Popularity     | `int`    | The games popularity level |
| PopularityRank | `int`    | The games popularity rank  |
| Rules          | `string` | The games rules (HTML)     |

---

#### Leaderboards

To fetch a leaderboard for a game from SRL you need to know the games abbrevation.

```c#
Leaderboard board = Client.Leaderboards.Get("sms");
```

or

```c#
Leaderboard board = Client.Leaderbaords["sms"];
```

##### Properties (Leaderboard)

| Property      | Type                         | Description               |
| ------------- | ---------------------------- | ------------------------- |
| Game          | `Endpoints.Games.Game`       | The associated game       |
| LeadersCount  | `int`                        | Count of ranked players   |
| Leaders       | `ReadOnlyCollection<Leader>` | List of ranked leaders    |
| UnrankedCount | `int`                        | Count of unranked players |
| Unranked      | `ReadOnlyCollection<Leader>` | List of unranked leaders  |

##### Properties (Leader)

| Property  | Type     | Description      |
| --------- | -------- | ---------------- |
| Name      | `string` | Player Name      |
| TrueSkill | `int`    | Trueskill value  |
| Rank      | `int`    | Leaderboard Rank |

---

#### Races

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

##### Properties (Race)

| Property    | Type                          | Description                                       |
| ----------- | ----------------------------- | ------------------------------------------------- |
| Id          | `string`                      | Race Id                                           |
| Game        | `Endpoints.Games.Game`        | The associated game                               |
| Goal        | `string`                      | Race Goal                                         |
| Time        | `int`                         | Total race time (after the last entrant finishes) |
| State       | `Endpoints.RaceState`         | Current state of the race                         |
| StateText   | `string`                      | Readable version of the current state             |
| FileName    | `string`                      | Filename used (if set via racebot)                |
| NumEntrants | `int`                         | Count of entrants                                 |
| Entrants    | `ReadOnlyCollection<Entrant>` | List of entrants                                  |

##### Properties (Entrant)

| Property  | Type                   | Description                           |
| --------- | ---------------------- | ------------------------------------- |
| Name      | `string`               | Player Name                           |
| Place     | `int`                  | Place (>0 if finished)                |
| Time      | `string`               | Final Time (>0 if finished)           |
| State     | `Endpoints.RacerState` | Current state of the entrant          |
| StateText | `string`               | Readable version of the current state |
| Twitch    | `string`               | Twitch channel                        |
| TrueSkill | `int`                  | Current TrueSkill value               |

---

#### PastRaces

To get a specific race you can use the following:

```c#
PastRace pastRace = Client.PastRaces.Get("raceid");
```

You can filter past races by player and game:

```c#
// Get my past races
ReadOnlyCollection<PastRace> pastRaces =  GetByPlayer("psychonauter");

// Get my past races in ffx
ReadOnlyCollection<PastRace> pastRaces = GetByPlayer("psychonauter", "ffx");
```

##### Properties (Race)

| Property      | Type                         | Description                             |
| ------------- | ---------------------------- | --------------------------------------- |
| Id            | `string`                     | Race Id                                 |
| Game          | `Endpoints.Games.Game`       | The associated game                     |
| Goal          | `string`                     | Race Goal                               |
| Date          | `int`                        | Timestamp of when the race was recorded |
| NumEntrants   | `int`                        | Count of entrants                       |
| Results       | `ReadOnlyCollection<Result>` | List of individual results              |
| RankedResults | `ReadOnlyCollection<Result>` | List of individual ranked results       |

##### Properties (Result)

| Property           | Type     | Description                                        |
| ------------------ | -------- | -------------------------------------------------- |
| Race               | `int`    | Race Id                                            |
| Place              | `int`    | Players place                                      |
| PlayerName         | `string` | Players Name                                       |
| Time               | `int`    | Players Final Time                                 |
| Comment            | `string` | Players Comment                                    |
| OldTrueSkill       | `int`    | TrueSkill value before the race was recorded       |
| NewTrueSkill       | `int`    | TrueSkill value after the race was recorded        |
| TrueSkillChange    | `int`    | TrueSkill difference                               |
| OldSeasonTrueSkill | `int`    | Season TrueSkill before the race was recorded      |
| NewTrueSkill       | `int`    | Season TrueSkill value after the race was recorded |
| TrueSkillChange    | `int`    | Season TrueSkill difference                        |

---

#### Players

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

##### Properties

| Property  | Type                 | Description                      |
| --------- | -------------------- | -------------------------------- |
| Id        | `int`                | Player Id                        |
| Name      | `string`             | Player name                      |
| Channel   | `string`             | Channel name (Twitch / Hitbox)   |
| Twitter   | `string`             | Twitter channel                  |
| Youtube   | `string`             | Youtube channel                  |
| StreamApi | `Entrants.StreamApi` | Api used for fetching the stream |
| Country   | `string`             | Player Country                   |

---
