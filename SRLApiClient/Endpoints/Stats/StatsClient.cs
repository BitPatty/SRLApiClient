using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SRLApiClient.Exceptions;

namespace SRLApiClient.Endpoints.Stats
{
  /// <summary>
  /// A client to perform requests on the /stat endpoint
  /// </summary>
  public class StatsClient : SRLEndpoint
  {
    /// <summary>
    /// Creates a new client to perform requests on the /stat endpoint
    /// </summary>
    /// <param name="baseClient">The <see cref="SRLClient"/> used to perform requests</param>
    public StatsClient(SRLClient baseClient) : base("/stat", baseClient) { }

    [DataContract, KnownType(typeof(SRLData))]
    private sealed class PlayerEndpoint : SRLData
    {
      [DataMember(Name = "stats", IsRequired = true)]
      public PlayerStats Stats { get; private set; }
    }

    /// <summary>
    /// Gets the statistics for a player synchronously
    /// </summary>
    /// <param name="playerName">The players name</param>
    /// <returns>Returns the players stats</returns>
    public PlayerStats GetPlayerStats(string playerName)
    {
      if (string.IsNullOrWhiteSpace(playerName)) throw new ArgumentException(nameof(playerName), "Parameter can't be empty");

      try
      {
        return SrlClient.Get<PlayerEndpoint>($"{BasePath}?player={playerName}")?.Stats;
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets the statistics for a player asynchronously
    /// </summary>
    /// <param name="playerName">The players name</param>
    /// <returns>Returns the players stats</returns>
    public async Task<PlayerStats> GetPlayerStatsAsync(string playerName)
    {
      if (string.IsNullOrWhiteSpace(playerName)) throw new ArgumentException(nameof(playerName), "Parameter can't be empty");

      try
      {
        return (await SrlClient.GetAsync<PlayerEndpoint>($"{BasePath}?player={playerName}").ConfigureAwait(false))?.Stats;
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets the statistics for a player in the specified game synchronously
    /// </summary>
    /// <param name="playerName">The players name</param>
    /// <param name="gameAbbreviation">The games abbreviation</param>
    /// <returns>Returns the players stats</returns>
    public PlayerStats GetPlayerStats(string playerName, string gameAbbreviation)
    {
      if (string.IsNullOrWhiteSpace(playerName)) throw new ArgumentException(nameof(playerName), "Parameter can't be empty");
      if (string.IsNullOrWhiteSpace(gameAbbreviation)) throw new ArgumentException(nameof(gameAbbreviation), "Parameter can't be empty");

      try
      {
        return SrlClient.Get<PlayerEndpoint>($"{BasePath}?player={playerName}&game={gameAbbreviation}")?.Stats;
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets the statistics for a player in the specified game asynchronously
    /// </summary>
    /// <param name="playerName">The players name</param>
    /// <param name="gameAbbreviation">The games abbreviation</param>
    /// <returns>Returns the players stats</returns>
    public async Task<PlayerStats> GetPlayerStatsAsync(string playerName, string gameAbbreviation)
    {
      if (string.IsNullOrWhiteSpace(playerName)) throw new ArgumentException(nameof(playerName), "Parameter can't be empty");
      if (string.IsNullOrWhiteSpace(gameAbbreviation)) throw new ArgumentException(nameof(gameAbbreviation), "Parameter can't be empty");

      try
      {
        return (await SrlClient.GetAsync<PlayerEndpoint>($"{BasePath}?player={playerName}&game={gameAbbreviation}").ConfigureAwait(false))?.Stats;
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    [DataContract, KnownType(typeof(SRLData))]
    private sealed class GameEndpoint : SRLData
    {
      [DataMember(Name = "stats", IsRequired = true)]
      public GameStats Stats { get; private set; }
    }

    /// <summary>
    /// Gets the statistics for a single game synchronously
    /// </summary>
    /// <param name="gameAbbreviation">The games abbreviation</param>
    /// <returns>Returns the games stats</returns>
    public GameStats GetGameStats(string gameAbbreviation)
    {
      if (string.IsNullOrWhiteSpace(gameAbbreviation)) throw new ArgumentException(nameof(gameAbbreviation), "Parameter can't be empty");

      try
      {
        return SrlClient.Get<GameEndpoint>($"{BasePath}?game={gameAbbreviation}")?.Stats;
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets the statistics for a single game asynchronously
    /// </summary>
    /// <param name="gameAbbreviation">The games abbreviation</param>
    /// <returns>Returns the games stats</returns>
    public async Task<GameStats> GetGameStatsAsync(string gameAbbreviation)
    {
      if (string.IsNullOrWhiteSpace(gameAbbreviation)) throw new ArgumentException(nameof(gameAbbreviation), "Parameter can't be empty");

      try
      {
        return (await SrlClient.GetAsync<GameEndpoint>($"{BasePath}?game={gameAbbreviation}").ConfigureAwait(false))?.Stats;
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    [DataContract, KnownType(typeof(SRLData))]
    private sealed class MonthlyStats : SRLData
    {
      [DataMember(Name = "monthlyStats", IsRequired = true)]
      public List<SRLStats> Stats { get; private set; }
    }

    /// <summary>
    /// Gets the monthly statistics synchronously
    /// </summary>
    /// <returns>Returns SRLs monthly stats</returns>
    public ReadOnlyCollection<SRLStats> GetMonthlySRLStats()
    {
      try
      {
        return SrlClient.Get<MonthlyStats>($"{BasePath}/monthly")?.Stats?.AsReadOnly();
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets the monthly statistics asynchronously
    /// </summary>
    /// <returns>Returns SRLs monthly stats</returns>
    public async Task<ReadOnlyCollection<SRLStats>> GetMonthlySRLStatsAsync()
    {
      try
      {
        return (await SrlClient.GetAsync<MonthlyStats>($"{BasePath}/monthly").ConfigureAwait(false))?.Stats?.AsReadOnly();
      }
      catch (SRLParseException)
      {
        return null;
      }
    }
  }
}
