﻿using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Games
{
  /// <summary>
  /// Game object
  /// </summary>
  [DataContract, KnownType(typeof(SRLDataType))]
  public class Game : SRLDataType
  {
    /// <summary>
    /// The games id
    /// </summary>
    [DataMember(Name = "id", IsRequired = true)]
    public int Id { get; protected set; }

    /// <summary>
    /// Full name of the game
    /// </summary>
    [DataMember(Name = "name", IsRequired = true)]
    public string Name { get; protected set; }

    /// <summary>
    /// SRL abbreviation of the game, used globally for indexing
    /// </summary>
    [DataMember(Name = "abbrev", IsRequired = true)]
    public string Abbreviation { get; protected set; }

    /// <summary>
    /// The games popularity level
    /// </summary>
    [DataMember(Name = "popularity", IsRequired = true)]
    public int Popularity { get; protected set; }

    /// <summary>
    /// The games popularity rank
    /// </summary>
    [DataMember(Name = "popularityrank", IsRequired = true)]
    public int PopularityRank { get; protected set; }

    /// <summary>
    /// The games rules
    /// </summary>
    public string Rules { get; internal set; }
  }
}
