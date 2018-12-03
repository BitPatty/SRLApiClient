﻿using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using SRLApiClient;
using SRLApiClient.Endpoints.Players;
using SRLApiClient.Endpoints.PastRaces;
using SRLApiClient.Endpoints.Races;
using SRLApiClient.Endpoints.Games;
using SRLApiClient.Endpoints.Leaderboards;
using SRLApiClient.Endpoints.Stats;

namespace Tests
{
  public class Tests
  {
    internal SRLClient _client { get; set; }

    [SetUp]
    public void Setup()
    {
      _client = new SRLClient();
    }

    [Test]
    public void TestCountriesEndpoint()
    {
      ReadOnlyCollection<string> countries = _client.Countries.Get();
      Assert.Contains("Switzerland", countries);
    }

    [Test]
    public void TestGamesEndpoint()
    {
      Game game = _client.Games["sms"];
      Assert.AreEqual(game.Name, "Super Mario Sunshine");
      Assert.IsNotNull(game.Rules);
      Assert.IsNotEmpty(game.Rules);
    }

    [Test]
    public void TestRacesEndpoint()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      Race race = _client.Races.Get(races[0].Id);
      Assert.AreEqual(race.Id, races[0].Id);
      Assert.IsNotNull(race.Game.Rules);
      Assert.IsNotEmpty(race.Game.Rules);
    }

    [Test]
    public void TestRacesPlayersEndpoints()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      Assert.GreaterOrEqual(races.Count, 0);
      Race tRace = races.FirstOrDefault(r => r.Entrants.Count > 0);
      Assert.IsNotNull(tRace);
      Player tPlayer = _client.Players.Get(tRace.Entrants[0].Name);
      Assert.AreEqual(tRace.Entrants[0].Twitch, tPlayer.Channel);
    }

    [Test]
    public void TestPastRacesPlayersEndpoints()
    {
      PastRace pastRace = _client.PastRaces.Get("239545");
      Assert.AreEqual(pastRace.Results.Count, 2);
      Player tPlayer = _client.Players.Get(pastRace.Results[0].PlayerName);
      Assert.IsNotNull(tPlayer);
      Assert.AreEqual(tPlayer.Name, pastRace.Results[0].PlayerName);
    }

    [Test]
    public void TestLeaderboardsEndpoint()
    {
      Leaderboard leaderboard = _client.Leaderboards.Get("sms");
      Assert.IsNotNull(leaderboard.Game.Rules);
      Assert.IsNotEmpty(leaderboard.Game.Rules);
      Assert.AreEqual(leaderboard.Leaders.Count, leaderboard.LeadersCount);
      Assert.AreEqual(leaderboard.Unranked.Count, leaderboard.UnrankedCount);
      Assert.AreEqual(leaderboard.Leaders[0].Rank, 1);
    }

    [Test]
    public void TestMonthlySRLStats()
    {
      ReadOnlyCollection<SRLStats> stats = _client.Stats.GetSRLStats();
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstOrDefault(s => s.Month == 10 && s.Year == 2018)?.TotalTimeRaced, 15320129);
    }

    [Test]
    public void TestGameStats()
    {
      GameStats stats = _client.Stats.GetGameStats("sms");
      Assert.IsNotNull(stats);
      Assert.GreaterOrEqual(stats.LargestRaceSize, 45);
    }

    [Test]
    public void TestPlayerStats()
    {
      PlayerStats stats = _client.Stats.GetPlayerStats("psychonauter");
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstRace, 167799);
    }

    [Test]
    public void TestPlayerGameStats()
    {
      PlayerStats stats = _client.Stats.GetPlayerStats("psychonauter", "ffx");
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstRace, 172802);
    }
  }
}
