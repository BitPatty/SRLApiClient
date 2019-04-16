using System.Collections.Generic;
using System.Runtime.Serialization;
using SRLApiClient.Endpoints.Games;

namespace SRLApiClient.Endpoints.PastRaces
{
  /// <summary>
  /// PastRace object
  /// </summary>
  [DataContract, KnownType(typeof(SRLData))]
  public class PastRace : SRLData
  {
    /// <summary>
    /// The race id
    /// </summary>
    [DataMember(Name = "id", IsRequired = true)]
    public string Id { get; protected set; }

    /// <summary>
    /// The game associated with the race
    /// </summary>
    [DataMember(Name = "game", IsRequired = true)]
    public Game Game { get; protected set; }

    /// <summary>
    /// The race goal
    /// </summary>
    [DataMember(Name = "goal", IsRequired = true)]
    public string Goal { get; protected set; }

    /// <summary>
    /// The date when the race was recorded
    /// </summary>
    [DataMember(Name = "date", IsRequired = true)]
    public string Date { get; protected set; }

    /// <summary>
    /// The number of entrants in the race
    /// </summary>
    [DataMember(Name = "numentrants", IsRequired = true)]
    public int NumEntrants { get; protected set; }

    /// <summary>
    /// The list of individual racer results
    /// </summary>
    [DataMember(Name = "results", IsRequired = true)]
    public List<Result> Results { get; protected set; }

    /// <summary>
    /// The list of individual ranked racer results
    /// </summary>
    [DataMember(Name = "ranked_results", IsRequired = true)]
    public List<Result> RankedResults { get; protected set; }
  }
}
