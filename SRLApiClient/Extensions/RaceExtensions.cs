﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using SRLApiClient.Endpoints;
using SRLApiClient.Endpoints.Races;

namespace SRLApiClient.Extensions
{
  /// <summary>
  /// Extensions for the <see cref="Race"/> type
  /// </summary>
  public static class RaceExtensions
  {
    /// <summary>
    /// Filter the race collection by a game abbreviation
    /// </summary>
    /// <param name="races">The original collection to apply the filter on</param>
    /// <param name="gameAbbrevation">The games abbrevation look for</param>
    /// <returns>Returns the races with the provided game abbreviation</returns>
    public static ReadOnlyCollection<Race> FilterByGame(this ReadOnlyCollection<Race> races, string gameAbbrevation)
      => races.Where(r => r.Game.Abbrevation.Equals(gameAbbrevation, StringComparison.OrdinalIgnoreCase)).ToList().AsReadOnly();

    /// <summary>
    /// Filter the race collection by the race state
    /// </summary>
    /// <param name="races">The original collection to apply the filter on</param>
    /// <param name="state">The games abbrevation to look for</param>
    /// <returns>Returns the races with the provided race state</returns>
    public static ReadOnlyCollection<Race> FilterByState(this ReadOnlyCollection<Race> races, RaceState state)
      => races.Where(r => r.State.Equals(state)).ToList().AsReadOnly();

    /// <summary>
    /// Filter the race collection by an entrants name
    /// </summary>
    /// <param name="races">The original collection to apply the filter on</param>
    /// <param name="playerName">The entrants name</param>
    /// <returns>Returns the races the provided player joined</returns>
    public static ReadOnlyCollection<Race> FilterByEntrant(this ReadOnlyCollection<Race> races, string playerName)
      => races.Where(r => r.Entrants.Any(e => e.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase))).ToList().AsReadOnly();

    /// <summary>
    /// Checks for a race id in the race collection
    /// </summary>
    /// <param name="races">The collection to apply the filter on</param>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the race or null if it wasn't found</returns>
    public static Race FilterById(this ReadOnlyCollection<Race> races, string raceId)
      => races.FirstOrDefault(r => r.Id.Equals(raceId, StringComparison.OrdinalIgnoreCase));
  }
}
