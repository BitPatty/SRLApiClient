using SRLApiClient.Endpoints.Games;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Leaderboards
{
  /// <summary>
  /// Leaderboard object
  /// </summary>
  [DataContract, KnownType(typeof(SRLDataType))]
  public class Leaderboard : SRLDataType
  {
    /// <summary>
    /// The game associated with the leaderboard
    /// </summary>
    [DataMember(Name = "game", IsRequired = true)]
    public Game Game { get; protected set; }

    /// <summary>
    /// The count of ranked players
    /// </summary>
    [DataMember(Name = "leadersCount", IsRequired = true)]
    public int LeadersCount { get; protected set; }

    /// <summary>
    /// The list of ranked players
    /// </summary>
    [DataMember(Name = "leaders", IsRequired = true)]
    protected List<Leader> _leaders { get; set; }

    /// <summary>
    /// The list of ranked players
    /// </summary>
    public ReadOnlyCollection<Leader> Leaders => _leaders.AsReadOnly();

    /// <summary>
    /// The count of unranked players
    /// </summary>
    [DataMember(Name = "unrankedCount", IsRequired = true)]
    public int UnrankedCount { get; protected set; }

    /// <summary>
    /// The list of unranked players
    /// </summary>
    [DataMember(Name = "unranked", IsRequired = true)]
    protected List<Leader> _unranked { get; set; }

    /// <summary>
    /// The list of unranked players
    /// </summary>
    public ReadOnlyCollection<Leader> Unranked => _unranked.AsReadOnly();
  }
}
