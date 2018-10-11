using SRLApiClient.Endpoints.Games;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Stats
{
  [DataContract, KnownType(typeof(SRLDataType))]
  public class PlayerStats : SRLDataType
  {
    /// <summary>
    /// Rank the associated games leaderboard (if provided)
    /// </summary>
    [DataMember(Name ="rank", IsRequired = true)]
    public int Rank { get; protected set; }

    /// <summary>
    /// Total races joined
    /// </summary>
    [DataMember(Name = "totalRaces", IsRequired = true)]
    public int TotalRaces { get; protected set; }

    /// <summary>
    /// Id of the first race
    /// </summary>
    [DataMember(Name = "firstRace", IsRequired = true)]
    public int FirstRace { get; protected set; }

    /// <summary>
    /// Date of the first race
    /// </summary>
    [DataMember(Name = "firstRaceDate", IsRequired = true)]
    public int FirstRaceDate { get; protected set; }

    /// <summary>
    /// Total accumulated play time
    /// </summary>
    [DataMember(Name = "totalTimePlayed", IsRequired = true)]
    public int TotalTimePlayed { get; protected set; }

    /// <summary>
    /// Total first places in race results 
    /// </summary>
    [DataMember(Name = "totalFirstPlace", IsRequired = true)]
    public int TotalFirstPlace { get; protected set; }

    /// <summary>
    /// Total second places in race results
    /// </summary>
    [DataMember(Name = "totalSecondPlace", IsRequired = true)]
    public int TotalSecondPlace { get; protected set; }

    /// <summary>
    /// Total third places in race results
    /// </summary>
    [DataMember(Name = "totalThirdPlace", IsRequired = true)]
    public int TotalThirdPlace { get; protected set; }

    /// <summary>
    /// Total quits/forfeits in race results
    /// </summary>
    [DataMember(Name = "totalQuits", IsRequired = true)]
    public int TotalQuits { get; protected set; }

    /// <summary>
    /// Total disqualifications in race results
    /// </summary>
    [DataMember(Name = "totalDisqualifications", IsRequired = true)]
    public int TotalDisqualifications { get; protected set; }
  }
}
