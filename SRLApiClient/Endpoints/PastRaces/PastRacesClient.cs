using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SRLApiClient.Endpoints.PastRaces
{
  /// <summary>
  /// A client to perform requests on the /pastraces endpoint
  /// </summary>
  public class PastRacesClient : SRLEndpoint
  {
    /// <summary>
    /// Creates a new client to perform requests on the /pastraces endpoint
    /// </summary>
    /// <param name="baseClient">The <see cref="SRLClient"/> used to perform requests</param>
    public PastRacesClient(SRLClient baseClient) : base("/pastraces", baseClient) { }

    /// <summary>
    /// Gets a single past race synchronously
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the past race</returns>
    public PastRace Get(string raceId)
    {
      if (String.IsNullOrWhiteSpace(raceId)) throw new ArgumentException(nameof(raceId), "Parameter can't be empty");

      try
      {
        return SrlClient.Get<PastRacesCollection>($"{BasePath}/{raceId.ToLower()}")?.Races?.FirstOrDefault();
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets a single past race asynchronously
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the past race</returns>
    public async Task<PastRace> GetAsync(string raceId)
    {
      if (String.IsNullOrWhiteSpace(raceId)) throw new ArgumentException(nameof(raceId), "Parameter can't be empty");

      try
      {
        return (await SrlClient.GetAsync<PastRacesCollection>($"{BasePath}/{raceId.ToLower()}").ConfigureAwait(false))?.Races?.FirstOrDefault();
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    [DataContract]
    private sealed class PastRacesCollection : SRLDataType
    {
      [DataMember(Name = "count", IsRequired = true)]
      public int Count { get; private set; }

      [DataMember(Name = "pastraces", IsRequired = true)]
      public List<PastRace> Races { get; private set; }
    }

    /// <summary>
    /// Gets all past races from a player synchronously
    /// </summary>
    /// <param name="playerName">The players name</param>
    /// <returns>Returns a list of past races</returns>
    public ReadOnlyCollection<PastRace> GetByPlayer(string playerName)
    {
      if (String.IsNullOrWhiteSpace(playerName)) throw new ArgumentException(nameof(playerName), "Parameter can't be empty");

      try
      {
        return SrlClient.Get<PastRacesCollection>($"{BasePath}?player={playerName}")?.Races.AsReadOnly();
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets all past races from a player in the specified game synchronously
    /// </summary>
    /// <param name="playerName">The players name</param>
    /// <param name="gameAbbreviation">The game</param>
    /// <returns>Returns a list of past races</returns>
    public ReadOnlyCollection<PastRace> GetByPlayer(string playerName, string gameAbbreviation)
    {
      if (String.IsNullOrWhiteSpace(playerName)) throw new ArgumentException(nameof(playerName), "Parameter can't be empty");
      if (String.IsNullOrWhiteSpace(gameAbbreviation)) throw new ArgumentException(nameof(gameAbbreviation), "Parameter can't be empty");

      try
      {
        return SrlClient.Get<PastRacesCollection>($"{BasePath}?player={playerName}&game={gameAbbreviation.ToLower()}")?.Races.AsReadOnly();
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets all past races from a player asynchronously
    /// </summary>
    /// <param name="playerName">The players name</param>
    /// <returns>Returns a list of past races</returns>
    public async Task<ReadOnlyCollection<PastRace>> GetByPlayerAsync(string playerName)
    {
      if (String.IsNullOrWhiteSpace(playerName)) throw new ArgumentException(nameof(playerName), "Parameter can't be empty");

      try
      {
        return (await SrlClient.GetAsync<PastRacesCollection>($"{BasePath}?player={playerName}").ConfigureAwait(false))?.Races.AsReadOnly();
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets all past races from a player in the specified game asynchronously
    /// </summary>
    /// <param name="playerName">The players name</param>
    /// <param name="gameAbbreviation">The game</param>
    /// <returns>Returns a list of past races</returns>
    public async Task<ReadOnlyCollection<PastRace>> GetByPlayerAsync(string playerName, string gameAbbreviation)
    {
      if (String.IsNullOrWhiteSpace(playerName)) throw new ArgumentException(nameof(playerName), "Parameter can't be empty");
      if (String.IsNullOrWhiteSpace(gameAbbreviation)) throw new ArgumentException(nameof(gameAbbreviation), "Parameter can't be empty");

      try
      {
        return (await SrlClient.GetAsync<PastRacesCollection>($"{BasePath}?player={playerName}&game={gameAbbreviation.ToLower()}").ConfigureAwait(false))?.Races.AsReadOnly();
      }
      catch (SRLParseException)
      {
        return null;
      }
    }
  }
}
