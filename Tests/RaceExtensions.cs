﻿using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using SRLApiClient.Endpoints;
using SRLApiClient.Endpoints.Races;
using SRLApiClient.Extensions;

namespace Tests
{
  public class RaceExtensions : BaseTest
  {
    [Test]
    [Category(nameof(RaceExtensions))]
    [Parallelizable]
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
    [Category(nameof(RaceExtensions))]
    [Parallelizable]
    public void FilterByGame()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      string abbr = races[0].Game.Abbreviation;
      ReadOnlyCollection<Race> matches = races.FilterByGame(abbr);
      Assert.Greater(matches.Count, 0);
      Assert.AreEqual(matches.Count, races.Count(r => r.Game.Abbreviation.Equals(abbr)));

      matches = races.FilterByGame("nonExistingGame");
      Assert.AreEqual(matches.Count, 0);
    }

    [Test]
    [Category(nameof(RaceExtensions))]
    [Parallelizable]
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
    [Category(nameof(RaceExtensions))]
    [Parallelizable]
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

    [Test]
    [Category(nameof(RaceExtensions))]
    [Parallelizable]
    public void FilterByMinState()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      RaceState state = races[0].State;
      ReadOnlyCollection<Race> matches = races.FilterByMinState(state);
      Assert.Greater(matches.Count, 0);
      Assert.True(matches.All(m => m.State >= state));

      matches = races.FilterByState(RaceState.Unknown);
      Assert.AreEqual(0, matches.Count);
    }

    [Test]
    [Category(nameof(RaceExtensions))]
    [Parallelizable]
    public void FilterByMaxState()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      RaceState state = races[0].State;
      ReadOnlyCollection<Race> matches = races.FilterByMaxState(state);
      Assert.Greater(matches.Count, 0);
      Assert.True(matches.All(m => m.State <= state));

      matches = races.FilterByState(RaceState.Unknown);
      Assert.AreEqual(0, matches.Count);
    }
  }
}
