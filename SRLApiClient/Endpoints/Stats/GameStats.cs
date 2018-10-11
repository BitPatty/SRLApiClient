using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Stats
{
  [DataContract, KnownType(typeof(SRLDataType))]
  public class GameStats : SRLDataType
  {
    /// <summary>
    /// Total amount of completed races
    /// </summary>
    [DataMember(Name = "totalRaces", IsRequired = true)]
    public int TotalRaces { get; protected set; }

    /// <summary>
    /// Total amount of players
    /// </summary>
    [DataMember(Name = "totalPlayers", IsRequired = true)]
    public int TotalPlayers { get; protected set; }

    /// <summary>
    /// Date of when the largest race happened
    /// </summary>
    [DataMember(Name = "largestRace", IsRequired = true)]
    public int LargestRaceDate { get; protected set; }

    /// <summary>
    /// Total amount of players in the largest race
    /// </summary>
    [DataMember(Name = "largestRaceSize", IsRequired = true)]
    public int LargestRaceSize { get; protected set; }

    /// <summary>
    /// Total accumulated race time for the game
    /// </summary>
    [DataMember(Name = "totalRaceTime", IsRequired = true)]
    public int TotalRaceTime { get; protected set; }

    /// <summary>
    /// Total accumulated play time for the game
    /// </summary>
    [DataMember(Name = "totalTimePlayed", IsRequired = true)]
    public int TotalTimePlayed { get; protected set; }
  }
}
