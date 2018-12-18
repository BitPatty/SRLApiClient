using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using SRLApiClient;
using SRLApiClient.Endpoints;
using SRLApiClient.Endpoints.Races;
using SRLApiClient.Extensions;

namespace Tests
{
  public class RaceExtensions
  {
    internal SRLClient _client { get; set; }

    [SetUp]
    public void Setup()
    {
      _client = new SRLClient();
    }

    [Test]
    public void FilterByEntrant()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      string name = races.First(r => r.Entrants.Count > 0).Entrants[0].Name;
      ReadOnlyCollection<Race> matches = races.FilterByEntrant(name);
      Assert.Greater(matches.Count, 0);
      Assert.AreEqual(matches.Count, races.Count(r => r.Entrants.Any(e => e.Name.Equals(name))));

      matches = races.FilterByEntrant("nonExistingPlayer<");
      Assert.AreEqual(matches.Count, 0);
    }

    [Test]
    public void FilterByGame()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      string abbr = races[0].Game.Abbrevation;
      ReadOnlyCollection<Race> matches = races.FilterByGame(abbr);
      Assert.Greater(matches.Count, 0);
      Assert.AreEqual(matches.Count, races.Count(r => r.Game.Abbrevation.Equals(abbr)));

      matches = races.FilterByGame("nonExistingGame");
      Assert.AreEqual(matches.Count, 0);
    }

    [Test]
    public void FilterById()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      string id = races[0].Id;
      Race match = races.FilterById(id);
      Assert.NotNull(match);
      Assert.AreEqual(id, match.Id);

      match = races.FilterById("nonExistingId");
      Assert.Null(match);
    }

    [Test]
    public void FilterByState()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      RaceState state = races[0].State;
      ReadOnlyCollection<Race> matches = races.FilterByState(state);
      Assert.Greater(matches.Count, 0);
      Assert.True(matches.All(m => m.State == state));

      matches = races.FilterByState(RaceState.Unknown);
      Assert.AreEqual(0, matches.Count);
    }
  }
}
