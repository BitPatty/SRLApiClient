# SRL API Client

[![NuGet](https://img.shields.io/nuget/v/SRLApiClient.svg)](https://www.nuget.org/packages/SRLApiClient) [![AppVeyor branch](https://img.shields.io/appveyor/ci/bitpatty/srlapiclient/master.svg)](https://ci.appveyor.com/project/BitPatty/srlapiclient/branch/master) [![GitHub license](https://img.shields.io/badge/license-AGPLv3-blue.svg)](https://raw.githubusercontent.com/BitPatty/SRLApiClient/master/LICENSE)

This repository contains a compact .Net Standard 2.0 library for the [SpeedRunsLive](http://speedrunslive.com) API. The client is available via [NuGet](https://www.nuget.org/packages/SRLApiClient).

```bash
#.Net
dotnet add package SRLApiClient

#Package Manager
Install-Package SRLApiClient
```

---

## Requirements

The SRL API Client supports any platform that implements [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard#net-implementation-support), such as Mono or .NET Core.

### Dependencies

_none_

---

## Usage

### Index

- [SRLClient](#srlclient)
- [Authentication](#authentication)
- [Endpoints:](#endpoints)
  - [Countries](#countries)
  - [Games](#games)
  - [Leaderboards](#leaderboards)
  - [Races](#races)
  - [PastRaces](#pastraces)
  - [Players](#players)
  - [Stats](#stats)
- [Extensions](#extensions)
  - [Leaderboard Extensions](#leaderboard-extensions)
  - [Race Extensions](#race-extensions)

### Initialization

#### `SRLClient`

```c#
using SRLApiClient;
```

To create a client you can simply use the following statement:

```c#
SRLClient Client = new SRLClient();
```

To use the Client with a custom host that uses the SRL flow you can pass the host as parameter. The default host is `"speedrunslive.com"`

```c#
SRLClient Client = new SRLClient("example.com");
```

##### Properties

| Property        | Type                                        | Description                               |
| --------------- | ------------------------------------------- | ----------------------------------------- |
| Games           | `Endpoints.Games.GamesClient`               | Client for fetching games                 |
| Players         | `Endpoints.Players.PlayersClient`           | Client for fetching player data           |
| Leaderboards    | `Endpoints.Leaderboards.LeaderboardsClient` | Client for fetching leaderboard data      |
| Races           | `Endpoints.Races.RacesClient`               | Client for fetching races                 |
| PastRaces       | `Endpoints.PastRaces.PastRacesClient`       | Client for fetching past races            |
| Countries       | `Endpoints.Countries.CountriesClient`       | Client for fetching countries             |
| Stats           | `Endpoints.Stats.StatsClient`               | Client for fetching game and player stats |
| User            | `SRLUser`                                   | Account used for authenticated requests   |
| IsAuthenticated | `Boolean`                                   | True on successful user verification      |
| Host            | `String`                                    | The host used for requests                |
| RequestTimeout  | `TimeSpan`                                  | Timeout for HTTP requests                 |

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
Leaderboard board = Client.Leaderboards["sms"];
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

_Note:_ `GetActive()` generally returns all races whose state is lower than `RaceState.Over`. However, the API sometimes already removes races from the endpoint as soon as they reach `RaceState.Finished`. So if you're actively tracking the races make sure you fetch the single race if it no longer shows up in the results.

_Note 2:_ The race id from `Races` doesn't match the `PastRaces` id. When the game is recorded it receives a new id and gets dropped from `Races`.

To fetch a specific Race you need to know the race id:

```
Race race = Client.Races.Get("someId");
```

or

```c#
Race race = Client.Races["someId"];
```

##### Properties (Race)

| Property    | Type                          | Description                                       |
| ----------- | ----------------------------- | ------------------------------------------------- |
| Id          | `string`                      | Race Id                                           |
| Game        | `Endpoints.Games.Game`        | The associated game                               |
| Goal        | `string`                      | Race Goal                                         |
| Time        | `int`                         | Total race time (after the last entrant finishes) |
| State       | `RaceState`                   | Current state of the race                         |
| StateText   | `string`                      | Readable version of the current state             |
| FileName    | `string`                      | Filename used (if set via racebot)                |
| NumEntrants | `int`                         | Count of entrants                                 |
| Entrants    | `ReadOnlyCollection<Entrant>` | List of entrants                                  |


##### Properties (Entrant)

| Property  | Type         | Description                           |
| --------- | ------------ | ------------------------------------- |
| Name      | `string`     | Player Name                           |
| Place     | `int`        | Place (>0 if finished)                |
| Time      | `string`     | Final Time (>0 if finished)           |
| State     | `RacerState` | Current state of the entrant          |
| StateText | `string`     | Readable version of the current state |
| Twitch    | `string`     | Twitch channel                        |
| TrueSkill | `int`        | Current TrueSkill value               |

---

#### PastRaces

To get a specific race you can use the following:

```c#
PastRace pastRace = Client.PastRaces.Get("raceid");
```

You can filter past races by player and game:

```c#
// Get my past races
ReadOnlyCollection<PastRace> pastRaces =  Client.Races.GetByPlayer("psychonauter");

// Get my past races in ffx
ReadOnlyCollection<PastRace> pastRaces = Client.Races.GetByPlayer("psychonauter", "ffx");
```

##### Properties (PastRace)

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

| Property  | Type        | Description                      |
| --------- | ----------- | -------------------------------- |
| Id        | `int`       | Player Id                        |
| Name      | `string`    | Player name                      |
| Channel   | `string`    | Channel name (Twitch / Hitbox)   |
| Twitter   | `string`    | Twitter channel                  |
| Youtube   | `string`    | Youtube channel                  |
| StreamApi | `StreamApi` | Api used for fetching the stream |
| Country   | `string`    | Player Country                   |

---

#### Stats

The client supports three kinds of stats: Player stats, game stats and SRL stats, which are available via the following calls:

```c#
//Player Stats
PlayerStats playerStats = Client.Stats.GetPlayerStats("psychonauter");

//Game Stats
GameStats gameStats = Client.Stats.GetGameStats("sms");

//Player stats in the specificed game
PlayerStats playerGameStats = Client.Stats.GetPlayerStast("psychonauter","sms");

//SRL Stats
SRLStats srlStats = Client.Stats.GetSRLStats();
```

##### Properties

###### PlayerStats

| Property               | Type  | Description                                                  |
| ---------------------- | ----- | ------------------------------------------------------------ |
| Rank                   | `int` | Player Rank (if game is specified)                           |
| TotalRaces             | `int` | Total races [in the game]                                    |
| FirstRace              | `int` | ID of the first race [in the game]                           |
| FirstRaceDate          | `int` | Date of the first race [in the game]                         |
| TotalTimePlayed        | `int` | Total play time [in the game]                                |
| TotalFirstPlace        | `int` | How many times the player reached first place [in the game]  |
| TotalSecondPlace       | `int` | How many times the player reached second place [in the game] |
| TotalThirdPlace        | `int` | How many times the player reached third place [in the game]  |
| TotalQuits             | `int` | How many times the player forfeited [in the game]            |
| TotalDisqualifications | `int` | How many times the player got disqualified [in the game]     |

###### GameStats

| Property        | Type  | Description              |
| --------------- | ----- | ------------------------ |
| TotalRaces      | `int` | Total count of races     |
| TotalPlayers    | `int` | Total count of players   |
| LargestRaceId   | `int` | ID of the largest race   |
| LargestRaceSize | `int` | Size of the largest race |
| TotalTimeRaced  | `int` | Total time raced         |
| TotalTimePlayed | `int` | Total time played        |

###### SRLStats

| Property        | Type  | Description                               |
| --------------- | ----- | ----------------------------------------- |
| Month           | `int` | Month                                     |
| Year            | `int` | Year                                      |
| TotalRaces      | `int` | Total count of races during that period   |
| TotalPlayers    | `int` | Total count of players during that period |
| LargestRaceId   | `int` | ID of the largest race                    |
| LargestRaceSize | `int` | Size of the largest race                  |
| TotalTimeRaced  | `int` | Total time raced                          |
| TotalTimePlayed | `int` | Total time played                         |

---

### Extensions

The library currently contains a few extension methods to make it easier to filter API responses. You can include them by importing `SRLApiClient.Exensions`.

#### Leaderboard Extensions

##### `ContainsPlayer(string playerName)`

Checks if a leaderboard contains a player matching `playerName`

```c#
Leaderboard board = Client.Leaderboards.Get("sms");
board.ContainsPlayer("psychonauter") //true
```

##### `FindPlayer(string playerName)`

Checks if the player exists and then returns the Leader object of the player (or null if they're not on the leaderboard).

```c#
Leaderboard board = Client.Leaderboards.Get("sms");
Leader leader = board.FindPlayer("psychonauter");
```

---

#### Race Extensions

##### `FilterByGame(string gameAbbrevation)`

Returns the subset of races matching the `gameAbbrevation`.

```c#
//Returns all active Super Mario Sunshine races
ReadOnlyCollection<Race> races = Client.Races.GetActive().FilterByGame("sms");
```

##### `FilterByState(RaceState state)`

Returns the subset of races matching the `state`.

```c#
//Returns all active races that are in progress
ReadOnlyCollection<Race> races = Client.Races.GetActive().FilterByState(RaceState.InProgress);
```

##### `FilterByMinState(RaceState state)`

Returns the subset of races greater or equal the provided `state`.

```c#
//Returns all active races that are in progress, done or over
ReadOnlyCollection<Race> races = Client.Races.GetActive().FilterByMinState(RaceState.InProgress);
```

##### `FilterByMaxState(RaceState state)`

Returns the subset of races less or equal the provided `state`.

```c#
//Returns all active races that are open or closed for entry
ReadOnlyCollection<Race> races = Client.Races.GetActive().FilterByState(RaceState.InProgress);
```

##### `FilterByEntrant(string playerName)`

Returns the subset of races the player with the name `playerName` is participating in.

```c#
//Returns all active races I'm participating in
ReadOnlyCollection<Race> races = Client.Races.GetActive().FilterByEntrant("psychonauter");
```

##### `FilterById(string raceId)`

Returns the race matching the provided `raceId` or `null` if the race isn't in the collection.

```c#
//Returns the active race matching raceId if it exists
Race race = Client.Races.GetActive().FilterById("someId");
```
