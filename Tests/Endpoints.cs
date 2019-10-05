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
  public class Endpoints
  {
    internal static SRLClient _client { get; set; } = new SRLClient(poolSize: 10);

    [Test]
    [Category("Endpoints")]
    [Parallelizable]
    public void Countries()
    {
      ReadOnlyCollection<string> countries = _client.Countries.Get();
      Assert.Contains("Switzerland", countries);
    }

    [Test]
    [Category("Endpoints")]
    [Parallelizable]
    public void Games()
    {
      Game game = _client.Games.Get("sms");
      Assert.AreEqual(game.Name, "Super Mario Sunshine");
    }

    [Test]
    [Category("Endpoints")]
    [Parallelizable]
    public void Games_Bulk()
    {
      ReadOnlyCollection<Game> games = _client.Games.GetAll();
      Assert.GreaterOrEqual(games.Count, 5300);
    }

    [Test]
    [Category("Endpoints")]
    [Parallelizable]
    public void Games_Rules()
    {
      string rules = _client.Games.GetRules("sms");
      Assert.IsNotEmpty(rules);
    }

    [Test]
    [Category("Endpoints")]
    [Parallelizable]
    public void Races_Active()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      Race race = _client.Races.Get(races[0].Id);
      Assert.AreEqual(race.Id, races[0].Id);
    }

    [Test]
    [Category("Endpoints")]
    [Parallelizable]
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
    [Parallelizable]
    public void PastRaces_Paginated()
    {
      ReadOnlyCollection<PastRace> pastRaces = _client.PastRaces.Get();
      Assert.AreEqual(pastRaces.Count, 20);

      ReadOnlyCollection<PastRace> pastRacesAlt = _client.PastRaces.Get(pageSize: 10);
      Assert.AreEqual(pastRacesAlt?.Count, 10);

      ReadOnlyCollection<PastRace> pastRacesPage2 = _client.PastRaces.Get(page: 2);

      Assert.IsNotNull(pastRacesAlt.FirstOrDefault(p => p.Id == pastRaces.FirstOrDefault()?.Id));
      Assert.IsNull(pastRacesPage2.FirstOrDefault(p => p.Id == pastRaces.FirstOrDefault()?.Id));
    }

    [Test]
    [Category("Endpoints")]
    [Parallelizable]
    public void PastRaces_GameFilter()
    {
      foreach (PastRace pr in _client.PastRaces.Get(gameAbbreviation: "sms"))
        Assert.AreEqual(pr.Game.Abbreviation, "sms");
    }

    [Test]
    [Category("Endpoints")]
    [Parallelizable]
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
    [Parallelizable]
    public void Leaderboards()
    {
      Leaderboard leaderboard = _client.Leaderboards.Get("sms");
      Assert.AreEqual(leaderboard.Leaders.Count, leaderboard.LeadersCount);
      Assert.AreEqual(leaderboard.Unranked.Count, leaderboard.UnrankedCount);
      Assert.AreEqual(leaderboard.Leaders[0].Rank, 1);
    }

    [Test]
    [Category("Endpoints")]
    [Parallelizable]
    public void Stats_SRL()
    {
      ReadOnlyCollection<SRLStats> stats = _client.Stats.GetMonthlySRLStats();
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstOrDefault(s => s.Month == 10 && s.Year == 2018)?.TotalTimeRaced, 15320129);
    }

    [Test]
    [Category("Endpoints")]
    [Parallelizable]
    public void Stats_Game()
    {
      GameStats stats = _client.Stats.GetGameStats("sms");
      Assert.IsNotNull(stats);
      Assert.GreaterOrEqual(stats.LargestRaceSize, 45);
    }

    [Test]
    [Category("Endpoints")]
    [Parallelizable]
    public void Stats_Player()
    {
      PlayerStats stats = _client.Stats.GetPlayerStats("psychonauter");
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstRace, 167799);
    }

    [Test]
    [Category("Endpoints")]
    [Parallelizable]
    public void Stats_PlayerAndGame()
    {
      PlayerStats stats = _client.Stats.GetPlayerStats("psychonauter", "ffx");
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstRace, 172802);
    }
  }
}
