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
    /// <param name="gameAbbrevation">The games abbrevation</param>
    /// <returns>Returns the players stats or null</returns>
    public PlayerStats GetPlayerStats(string playerName, string gameAbbrevation = null)
    {
      if (string.IsNullOrWhiteSpace(playerName)) throw new ArgumentException(nameof(playerName), "Parameter can't be empty");
      if (gameAbbrevation == null) return SrlClient.Get(BasePath + "?player=" + playerName, out PlayerEndpoint ps) ? ps.Stats : null;
      else return SrlClient.Get(BasePath + "?player=" + playerName + "&game=" + gameAbbrevation, out PlayerEndpoint ps) ? ps.Stats : null;
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
    /// <param name="gameAbbrevation">The games abbrevation</param>
    /// <returns>Returns the games stats or null</returns>
    public GameStats GetGameStats(string gameAbbrevation)
    {
      if (string.IsNullOrWhiteSpace(gameAbbrevation)) throw new ArgumentException(nameof(gameAbbrevation), "Parameter can't be empty");
      return SrlClient.Get(BasePath + "?game=" + gameAbbrevation, out GameEndpoint ge) ? ge.Stats : null;
    }

    [DataContract, KnownType(typeof(SRLDataType))]
    private sealed class MonthlyStats : SRLDataType
    {
      [DataMember(Name = "monthlyStats", IsRequired = true)]
      public List<MonthlySRLStats> Stats { get; private set; }
    }

    /// <summary>
    /// Gets the monthly statistics of SRL
    /// </summary>
    /// <returns>Returns SRLs monthly stats or null</returns>
    public ReadOnlyCollection<MonthlySRLStats> GetSRLStats() =>
      SrlClient.Get(BasePath + "/monthly", out MonthlyStats ms) ? ms.Stats?.AsReadOnly() : null;
  }
}
