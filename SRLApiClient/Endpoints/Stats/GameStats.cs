using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Stats
{
  /// <summary>
  /// GameStats object
  /// </summary>
  [DataContract, KnownType(typeof(SRLDataType))]
  public class GameStats : SRLDataType
  {
    /// <summary>
    /// The games total race count
    /// </summary>
    [DataMember(Name = "totalRaces", IsRequired = true)]
    public int TotalRaces { get; protected set; }

    /// <summary>
    /// The games total player count
    /// </summary>
    [DataMember(Name = "totalPlayers", IsRequired = true)]
    public int TotalPlayers { get; protected set; }

    /// <summary>
    /// The ID of the largest race
    /// </summary>
    [DataMember(Name = "largestRace", IsRequired = true)]
    public int LargestRaceId { get; protected set; }

    /// <summary>
    /// The games total player count in the largest race
    /// </summary>
    [DataMember(Name = "largestRaceSize", IsRequired = true)]
    public int LargestRaceSize { get; protected set; }

    /// <summary>
    /// The games total accumulated race time for the game
    /// </summary>
    [DataMember(Name = "totalRaceTime", IsRequired = true)]
    public int TotalTimeRaced { get; protected set; }

    /// <summary>
    /// The games total accumulated play time
    /// </summary>
    [DataMember(Name = "totalTimePlayed", IsRequired = true)]
    public int TotalTimePlayed { get; protected set; }
  }
}
