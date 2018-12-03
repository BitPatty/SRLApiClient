using SRLApiClient.Endpoints.Games;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Stats
{
  /// <summary>
  /// PlayerStats object
  /// </summary>
  [DataContract, KnownType(typeof(SRLDataType))]
  public class PlayerStats : SRLDataType
  {
    /// <summary>
    /// The players rank on the games associated leaderboard (if queried)
    /// </summary>
    [DataMember(Name = "rank", IsRequired = true)]
    public int Rank { get; protected set; }

    /// <summary>
    /// The players total race caount
    /// </summary>
    [DataMember(Name = "totalRaces", IsRequired = true)]
    public int TotalRaces { get; protected set; }

    /// <summary>
    /// The ID of the players first race
    /// </summary>
    [DataMember(Name = "firstRace", IsRequired = true)]
    public int FirstRace { get; protected set; }

    /// <summary>
    /// The date of the players first race
    /// </summary>
    [DataMember(Name = "firstRaceDate", IsRequired = true)]
    public int FirstRaceDate { get; protected set; }

    /// <summary>
    /// The players accumulated play time (in the game if queried)
    /// </summary>
    [DataMember(Name = "totalTimePlayed", IsRequired = true)]
    public int TotalTimePlayed { get; protected set; }

    /// <summary>
    /// The players first place count in race results (of the game if queried)
    /// </summary>
    [DataMember(Name = "totalFirstPlace", IsRequired = true)]
    public int TotalFirstPlace { get; protected set; }

    /// <summary>
    /// The players second place count in race results (of the game if queried)
    /// </summary>
    [DataMember(Name = "totalSecondPlace", IsRequired = true)]
    public int TotalSecondPlace { get; protected set; }

    /// <summary>
    /// The players third place count in race results (of the game if queried)
    /// </summary>
    [DataMember(Name = "totalThirdPlace", IsRequired = true)]
    public int TotalThirdPlace { get; protected set; }

    /// <summary>
    /// The players forfeit count in race results (of the game if queried)
    /// </summary>
    [DataMember(Name = "totalQuits", IsRequired = true)]
    public int TotalQuits { get; protected set; }

    /// <summary>
    /// The players disqualification count in race results (of the game if queried)
    /// </summary>
    [DataMember(Name = "totalDisqualifications", IsRequired = true)]
    public int TotalDisqualifications { get; protected set; }
  }
}
