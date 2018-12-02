using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Stats
{
  /// <summary>
  /// Monthly SRL Stats
  /// </summary>
  [DataContract, KnownType(typeof(SRLDataType))]
  public sealed class MonthlySRLStats : SRLDataType
  {
    /// <summary>
    /// The month of the year of the associated stats
    /// </summary>
    [DataMember(Name = "month", IsRequired = true)]
    public int Month { get; private set; }

    /// <summary>
    /// The year of the associated stats
    /// </summary>
    [DataMember(Name = "year", IsRequired = true)]
    public int Year { get; private set; }

    /// <summary>
    /// The total amount of races in the associated month
    /// </summary>
    [DataMember(Name = "totalRaces", IsRequired = true)]
    public int TotalRaces { get; private set; }

    /// <summary>
    /// The total amount of players in the associated month
    /// </summary>
    [DataMember(Name = "totalPlayers", IsRequired = true)]
    public int TotalPlayers { get; private set; }

    /// <summary>
    /// The total amount of games played in the associated month
    /// </summary>
    [DataMember(Name = "totalGames", IsRequired = true)]
    public int TotalGames { get; private set; }

    /// <summary>
    /// The ID of the largest race in the associated month
    /// </summary>
    [DataMember(Name = "largestRace", IsRequired = true)]
    public int LargestRaceId { get; private set; }

    /// <summary>
    /// The amount of entrants in the largest race in the associated month
    /// </summary>
    [DataMember(Name = "largestRaceSize", IsRequired = true)]
    public int LargestRaceSize { get; private set; }

    /// <summary>
    /// The total race time in the associated month
    /// </summary>
    [DataMember(Name = "totalRaceTime", IsRequired = true)]
    public int TotalRaceTime { get; private set; }

    /// <summary>
    /// The total play time in the associated month
    /// </summary>
    [DataMember(Name = "totalTimePlayed", IsRequired = true)]
    public int TotalTimePlayed { get; private set; }
  }
}
