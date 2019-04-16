using System.Collections.ObjectModel;
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
  public class EndpointsAsync
  {
    internal SRLClient _client { get; set; }

    [SetUp]
    public void Setup()
    {
      _client = new SRLClient();
    }

    [Test]
    [Category("EndpointsAsync")]
    public void Countries()
    {
      ReadOnlyCollection<string> countries = _client.Countries.GetAsync().Result;
      Assert.Contains("Switzerland", countries);
    }

    [Test]
    [Category("EndpointsAsync")]
    public void Games()
    {
      Game game = _client.Games.GetAsync("sms").Result;
      Assert.AreEqual(game.Name, "Super Mario Sunshine");
    }

    [Test]
    [Category("EndpointsAsync")]
    public void Games_Rules()
    {
      string rules = _client.Games.GetRulesAsync("sms").Result;
      Assert.IsNotEmpty(rules);
    }

    [Test]
    [Category("EndpointsAsync")]
    public void Races_Active()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActiveAsync().Result;
      Race race = _client.Races.Get(races[0].Id);
      Assert.AreEqual(race.Id, races[0].Id);
    }

    [Test]
    [Category("EndpointsAsync")]
    public void RacesAndPlayers()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActiveAsync().Result;
      Assert.GreaterOrEqual(races.Count, 0);
      Race tRace = races.FirstOrDefault(r => r.Entrants.Count > 0);
      Assert.IsNotNull(tRace);

      Player tPlayer = _client.Players.GetAsync(tRace.Entrants[0].Name).Result;
      Assert.AreEqual(tRace.Entrants[0].Twitch, tPlayer.Channel);
    }

    [Test]
    [Category("EndpointsAsync")]
    public void PastRaces_Paginated()
    {
      ReadOnlyCollection<PastRace> pastRaces = _client.PastRaces.GetAsync().Result;
      Assert.AreEqual(pastRaces.Count, 20);

      ReadOnlyCollection<PastRace> pastRacesAlt = _client.PastRaces.GetAsync(pageSize: 10).Result;
      Assert.AreEqual(pastRacesAlt?.Count, 10);

      ReadOnlyCollection<PastRace> pastRacesPage2 = _client.PastRaces.GetAsync(page: 2).Result;

      Assert.IsNotNull(pastRacesAlt.FirstOrDefault(p => p.Id == pastRaces.FirstOrDefault()?.Id));
      Assert.IsNull(pastRacesPage2.FirstOrDefault(p => p.Id == pastRaces.FirstOrDefault()?.Id));
    }

    [Test]
    [Category("EndpointsAsync")]
    public void PastRaces_GameFilter()
    {
      foreach (PastRace pr in _client.PastRaces.GetAsync(gameAbbreviation: "sms").Result)
        Assert.AreEqual(pr.Game.Abbreviation, "sms");
    }

    [Test]
    [Category("EndpointsAsync")]
    public void PastRacesAndPlayers()
    {
      PastRace pastRace = _client.PastRaces.GetAsync("239545").Result;
      Assert.AreEqual(pastRace.Results.Count, 2);
      Player tPlayer = _client.Players.GetAsync(pastRace.Results[0].PlayerName).Result;
      Assert.IsNotNull(tPlayer);
      Assert.AreEqual(tPlayer.Name, pastRace.Results[0].PlayerName);
    }

    [Test]
    [Category("EndpointsAsync")]
    public void Leaderboards()
    {
      Leaderboard leaderboard = _client.Leaderboards.GetAsync("sms").Result;
      Assert.AreEqual(leaderboard.Leaders.Count, leaderboard.LeadersCount);
      Assert.AreEqual(leaderboard.Unranked.Count, leaderboard.UnrankedCount);
      Assert.AreEqual(leaderboard.Leaders[0].Rank, 1);
    }

    [Test]
    [Category("EndpointsAsync")]
    public void Stats_SRL()
    {
      ReadOnlyCollection<SRLStats> stats = _client.Stats.GetMonthlySRLStatsAsync().Result;
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstOrDefault(s => s.Month == 10 && s.Year == 2018)?.TotalTimeRaced, 15320129);
    }

    [Test]
    [Category("EndpointsAsync")]
    public void Stats_Game()
    {
      GameStats stats = _client.Stats.GetGameStatsAsync("sms").Result;
      Assert.IsNotNull(stats);
      Assert.GreaterOrEqual(stats.LargestRaceSize, 45);
    }

    [Test]
    [Category("EndpointsAsync")]
    public void Stats_Player()
    {
      PlayerStats stats = _client.Stats.GetPlayerStatsAsync("psychonauter").Result;
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstRace, 167799);
    }

    [Test]
    [Category("EndpointsAsync")]
    public void Stats_PlayerAndGame()
    {
      PlayerStats stats = _client.Stats.GetPlayerStatsAsync("psychonauter", "ffx").Result;
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstRace, 172802);
    }
  }
}
