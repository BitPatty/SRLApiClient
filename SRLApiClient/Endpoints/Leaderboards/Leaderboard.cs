﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SRLApiClient.Endpoints.Games;

namespace SRLApiClient.Endpoints.Leaderboards
{
  /// <summary>
  /// Leaderboard object
  /// </summary>
  [DataContract, KnownType(typeof(SRLData))]
  public class Leaderboard : SRLData
  {
    /// <summary>
    /// The game associated with the leaderboard
    /// </summary>
    [DataMember(Name = "game", IsRequired = true)]
    public Game Game { get; protected set; }

    /// <summary>
    /// The number of ranked players
    /// </summary>
    [DataMember(Name = "leadersCount", IsRequired = true)]
    public int LeadersCount { get; protected set; }

    /// <summary>
    /// The list of ranked players
    /// </summary>
    [DataMember(Name = "leaders", IsRequired = true)]
    private List<Leader> _leaders { get; set; }

    /// <summary>
    /// The list of ranked players
    /// </summary>
    public ReadOnlyCollection<Leader> Leaders => _leaders.AsReadOnly();

    /// <summary>
    /// The number of unranked players
    /// </summary>
    [DataMember(Name = "unrankedCount", IsRequired = true)]
    public int UnrankedCount { get; protected set; }

    /// <summary>
    /// The list of unranked players
    /// </summary>
    [DataMember(Name = "unranked", IsRequired = true)]
    private List<Leader> _unranked { get; set; }

    /// <summary>
    /// The list of unranked players
    /// </summary>
    public ReadOnlyCollection<Leader> Unranked => _unranked.AsReadOnly();
  }
}
