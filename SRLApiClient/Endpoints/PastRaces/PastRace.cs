using SRLApiClient.Endpoints.Games;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.PastRaces
{
  [DataContract, KnownType(typeof(SRLDataType))]
  public class PastRace : SRLDataType
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
    /// Date when the race was recorded
    /// </summary>
    [DataMember(Name = "date", IsRequired = true)]
    public string Date { get; protected set; }

    /// <summary>
    /// Number of entrants in the race
    /// </summary>
    [DataMember(Name = "numentrants", IsRequired = true)]
    public int NumEntrants { get; protected set; }

    /// <summary>
    /// List of individual racer results
    /// </summary>
    [DataMember(Name = "results", IsRequired = true)]
    public List<Result> Results { get; protected set; }

    /// <summary>
    /// List of individual ranked racer results
    /// </summary>
    [DataMember(Name = "ranked_results", IsRequired = true)]
    public List<Result> RankedResults { get; protected set; }

  }
}
