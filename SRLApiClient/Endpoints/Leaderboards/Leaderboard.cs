using SRLApiClient.Endpoints.Games;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Leaderboards
{
  [DataContract, KnownType(typeof(SRLDataType))]
  public class Leaderboard : SRLDataType
  {
    /// <summary>
    /// The game associated with the leaderboard
    /// </summary>
    [DataMember(Name = "game", IsRequired = true)]
    public Game Game { get; protected set; }

    /// <summary>
    /// Count of ranked players
    /// </summary>
    [DataMember(Name = "leadersCount", IsRequired = true)]
    public int LeadersCount { get; protected set; }

    [DataMember(Name = "leaders", IsRequired = true)]
    protected List<Leader> _leaders { get; set; }

    /// <summary>
    /// List of ranked players
    /// </summary>
    public ReadOnlyCollection<Leader> Leaders => _leaders.AsReadOnly();

    /// <summary>
    /// Count of unranked players
    /// </summary>
    [DataMember(Name = "unrankedCount", IsRequired = true)]
    public int UnrankedCount { get; protected set; }

    /// <summary>
    /// List of unranked players
    /// </summary>
    [DataMember(Name = "unranked", IsRequired = true)]
    protected List<Leader> _unranked { get; set; }
    public ReadOnlyCollection<Leader> Unranked => _unranked.AsReadOnly();
  }
}
