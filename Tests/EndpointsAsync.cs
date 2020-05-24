using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using SRLApiClient.Endpoints.Games;
using SRLApiClient.Endpoints.Leaderboards;
using SRLApiClient.Endpoints.PastRaces;
using SRLApiClient.Endpoints.Players;
using SRLApiClient.Endpoints.Races;
using SRLApiClient.Endpoints.Stats;

namespace Tests
{
  public class EndpointsAsync : BaseTest
  {
    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void Countries()
    {
      ReadOnlyCollection<string> countries = _client.Countries.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.Contains("Switzerland", countries);
    }

    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void Games()
    {
      Game game = _client.Games.GetAsync("sms").ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.AreEqual(game.Name, "Super Mario Sunshine");
    }

    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void Games_Rules()
    {
      string rules = _client.Games.GetRulesAsync("sms").ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.IsNotEmpty(rules);
    }

    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void Races_Active()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActiveAsync().ConfigureAwait(false).GetAwaiter().GetResult();
      Race race = _client.Races.Get(races[0].Id);
      Assert.AreEqual(race.Id, races[0].Id);
    }

    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void RacesAndPlayers()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActiveAsync().ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.GreaterOrEqual(races.Count, 0);
      Race tRace = races.FirstOrDefault(r => r.Entrants.Count > 0);
      Assert.IsNotNull(tRace);

      Player tPlayer = _client.Players.GetAsync(tRace.Entrants[0].Name).ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.AreEqual(tRace.Entrants[0].Twitch, tPlayer.Channel);
    }

    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void PastRaces_Paginated()
    {
      ReadOnlyCollection<PastRace> pastRaces = _client.PastRaces.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.AreEqual(pastRaces.Count, 20);

      ReadOnlyCollection<PastRace> pastRacesAlt = _client.PastRaces.GetAsync(pageSize: 10).ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.AreEqual(pastRacesAlt?.Count, 10);

      ReadOnlyCollection<PastRace> pastRacesPage2 = _client.PastRaces.GetAsync(page: 2).ConfigureAwait(false).GetAwaiter().GetResult();

      Assert.IsNotNull(pastRacesAlt.FirstOrDefault(p => p.Id == pastRaces.FirstOrDefault()?.Id));
      Assert.IsNull(pastRacesPage2.FirstOrDefault(p => p.Id == pastRaces.FirstOrDefault()?.Id));
    }

    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void PastRaces_GameFilter()
    {
      foreach (PastRace pr in _client.PastRaces.GetAsync(gameAbbreviation: "sms").ConfigureAwait(false).GetAwaiter().GetResult())
        Assert.AreEqual(pr.Game.Abbreviation, "sms");
    }

    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void PastRacesAndPlayers()
    {
      PastRace pastRace = _client.PastRaces.GetAsync("239545").ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.AreEqual(pastRace.Results.Count, 2);
      Player tPlayer = _client.Players.GetAsync(pastRace.Results[0].PlayerName).ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.IsNotNull(tPlayer);
      Assert.AreEqual(tPlayer.Name, pastRace.Results[0].PlayerName);
    }

    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void Leaderboards()
    {
      Leaderboard leaderboard = _client.Leaderboards.GetAsync("sms").ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.AreEqual(leaderboard.Leaders.Count, leaderboard.LeadersCount);
      Assert.AreEqual(leaderboard.Unranked.Count, leaderboard.UnrankedCount);
      Assert.AreEqual(leaderboard.Leaders[0].Rank, 1);
    }

    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void Stats_SRL()
    {
      ReadOnlyCollection<SRLStats> stats = _client.Stats.GetMonthlySRLStatsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstOrDefault(s => s.Month == 10 && s.Year == 2018)?.TotalTimeRaced, 15320129);
    }

    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void Stats_Game()
    {
      GameStats stats = _client.Stats.GetGameStatsAsync("sms").ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.IsNotNull(stats);
      Assert.GreaterOrEqual(stats.LargestRaceSize, 45);
    }

    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void Stats_Player()
    {
      PlayerStats stats = _client.Stats.GetPlayerStatsAsync("psychonauter").ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstRace, 167799);
    }

    [Test]
    [Category(nameof(EndpointsAsync))]
    [Parallelizable]
    public void Stats_PlayerAndGame()
    {
      PlayerStats stats = _client.Stats.GetPlayerStatsAsync("psychonauter", "ffx").ConfigureAwait(false).GetAwaiter().GetResult();
      Assert.IsNotNull(stats);
      Assert.AreEqual(stats.FirstRace, 172802);
    }
  }
}
