using System;
using System.Collections.Generic;
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
    /// <typeparam name="T"></typeparam>
    /// <param name="races">The original collection to apply the filter on</param>
    /// <param name="gameAbbrevation">The games abbrevation look for</param>
    /// <returns>Returns the races with the provided game abbreviation</returns>
    public static T FilterByGame<T>(this T races, string gameAbbrevation) where T : IEnumerable<Race>
      => (T)races.Where(r => r.Game.Abbrevation.Equals(gameAbbrevation, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Filter the race collection by the race state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="races">The original collection to apply the filter on</param>
    /// <param name="state">The games abbrevation to look for</param>
    /// <returns>Returns the races with the provided race state</returns>
    public static T FilterByState<T>(this T races, RaceState state) where T : IEnumerable<Race>
      => (T)races.Where(r => r.State.Equals(state));

    /// <summary>
    /// Filter the race collection by an entrants name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="races">The original collection to apply the filter on</param>
    /// <param name="playerName">The entrants name</param>
    /// <returns>Returns the races the provided player joined</returns>
    public static T FilterByEntrant<T>(this T races, string playerName) where T : IEnumerable<Race>
      => (T)races.Where(r => r.Entrants.FirstOrDefault(e => e.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase)) != null);

    /// <summary>
    /// Checks for a race id in the race collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="races">The collection to apply the filter on</param>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the race or null if it wasn't found</returns>
    public static Race FilterById<T>(this T races, string raceId) where T : IEnumerable<Race>
      => races.FirstOrDefault(r => r.Id.Equals(raceId, StringComparison.OrdinalIgnoreCase));
  }
}
