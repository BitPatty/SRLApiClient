using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

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

    [DataContract, KnownType(typeof(SRLDataType))]
    private sealed class PlayerEndpoint : SRLDataType
    {
      [DataMember(Name = "stats", IsRequired = true)]
      public PlayerStats Stats { get; private set; }
    }

    /// <summary>
    /// Gets the statistics for a player
    /// </summary>
    /// <param name="playerName">The players name </param>
    /// <param name="gameAbbreviation">The games abbreviation</param>
    /// <returns>Returns the players stats or null</returns>
    public PlayerStats GetPlayerStats(string playerName, string gameAbbreviation = null)
    {
      if (string.IsNullOrWhiteSpace(playerName)) throw new ArgumentException(nameof(playerName), "Parameter can't be empty");
      if (gameAbbreviation == null) return SrlClient.Get(BasePath + "?player=" + playerName, out PlayerEndpoint ps) ? ps.Stats : null;
      else return SrlClient.Get(BasePath + "?player=" + playerName + "&game=" + gameAbbreviation, out PlayerEndpoint ps) ? ps.Stats : null;
    }

    [DataContract, KnownType(typeof(SRLDataType))]
    private sealed class GameEndpoint : SRLDataType
    {
      [DataMember(Name = "stats", IsRequired = true)]
      public GameStats Stats { get; private set; }
    }

    /// <summary>
    /// Gets the statistics for a single game
    /// </summary>
    /// <param name="gameAbbreviation">The games abbreviation</param>
    /// <returns>Returns the games stats or null</returns>
    public GameStats GetGameStats(string gameAbbreviation)
    {
      if (string.IsNullOrWhiteSpace(gameAbbreviation)) throw new ArgumentException(nameof(gameAbbreviation), "Parameter can't be empty");
      return SrlClient.Get(BasePath + "?game=" + gameAbbreviation, out GameEndpoint ge) ? ge.Stats : null;
    }

    [DataContract, KnownType(typeof(SRLDataType))]
    private sealed class MonthlyStats : SRLDataType
    {
      [DataMember(Name = "monthlyStats", IsRequired = true)]
      public List<SRLStats> Stats { get; private set; }
    }

    /// <summary>
    /// Gets the monthly statistics of SRL
    /// </summary>
    /// <returns>Returns SRLs monthly stats or null</returns>
    public ReadOnlyCollection<SRLStats> GetSRLStats() =>
      SrlClient.Get(BasePath + "/monthly", out MonthlyStats ms) ? ms.Stats?.AsReadOnly() : null;
  }
}
