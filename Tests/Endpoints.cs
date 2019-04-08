﻿using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using SRLApiClient;
using SRLApiClient.Endpoints.Games;
using SRLApiClient.Endpoints.Leaderboards;
using SRLApiClient.Endpoints.PastRaces;
using SRLApiClient.Endpoints.Players;
using SRLApiClient.Endpoints.Races;
using SRLApiClient.Endpoints.Stats;

namespace Tests
{
  public class Endpoints
  {
    internal SRLClient _client { get; set; }

    [SetUp]
    public void Setup()
    {
      _client = new SRLClient();
    }

    [Test]
    [Category("Endpoints")]
    public void Countries()
    {
      ReadOnlyCollection<string> countries = _client.Countries.Get();
      Assert.Contains("Switzerland", countries);
    }

    [Test]
    [Category("Endpoints")]
    public void Games()
    {
      Game game = _client.Games.Get("sms");
      Assert.AreEqual(game.Name, "Super Mario Sunshine");
    }

    [Test]
    [Category("Endpoints")]
    public void GameRules()
    {
      string rules = _client.Games.GetRules("sms");
      Assert.IsNotEmpty(rules);
    }

    [Test]
    [Category("Endpoints")]
    public void Races()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      Race race = _client.Races.Get(races[0].Id);
      Assert.AreEqual(race.Id, races[0].Id);
    }

    [Test]
    [Category("Endpoints")]
    public void RacesAndPlayers()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      Assert.GreaterOrEqual(races.Count, 0);
      Race tRace = races.FirstOrDefault(r => r.Entrants.Count > 0);
      Assert.IsNotNull(tRace);
      Player tPlayer = _client.Players.Get(tRace.Entrants[0].Name);
      Assert.AreEqual(tRace.Entrants[0].Twitch, tPlayer.Channel);
    }

    [Test]
    [Category("Endpoints")]
    public void PastRacesAndPlayers()
    {
      PastRace pastRace = _client.PastRaces.Get("239545");
      Assert.AreEqual(pastRace.Results.Count, 2);
      Player tPlayer = _client.Players.Get(pastRace.Results[0].PlayerName);
      Assert.IsNotNull(tPlayer);
      Assert.AreEqual(tPlayer.Name, pastRace.Results[0].PlayerName);
    }

    [Test]
    [Category("Endpoints")]
    public void Leaderboards()
    {
      Leaderboard leaderboard = _client.Leaderboards.Get("sms");
      Assert.AreEqual(leaderboard.Leaders.Count, leaderboard.LeadersCount);
      Assert.AreEqual(leaderboard.Unranked.Count, leaderboard.UnrankedCount);
      Assert.AreEqual(leaderboard.Leaders[0].Rank, 1);
    }

    [Test]
    [Category("Endpoints")]
    public void MonthlySRLStats()
    {
      ReadOnlyCollection<SRLStats> stats = _client.Stats.GetMonthlySRLStats();
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstOrDefault(s => s.Month == 10 && s.Year == 2018)?.TotalTimeRaced, 15320129);
    }

    [Test]
    [Category("Endpoints")]
    public void GameStats()
    {
      GameStats stats = _client.Stats.GetGameStats("sms");
      Assert.IsNotNull(stats);
      Assert.GreaterOrEqual(stats.LargestRaceSize, 45);
    }

    [Test]
    [Category("Endpoints")]
    public void PlayerStats()
    {
      PlayerStats stats = _client.Stats.GetPlayerStats("psychonauter");
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstRace, 167799);
    }

    [Test]
    [Category("Endpoints")]
    public void PlayerGameStats()
    {
      PlayerStats stats = _client.Stats.GetPlayerStats("psychonauter", "ffx");
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstRace, 172802);
    }
  }
}
