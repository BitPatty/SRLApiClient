using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Games
{
  [DataContract, KnownType(typeof(SRLDataType))]
  public class Game : SRLDataType
  {
    /// <summary>
    /// The game ID
    /// </summary>
    [DataMember(Name = "id", IsRequired = true)]
    public int Id { get; protected set; }

    /// <summary>
    /// Full Name of the game
    /// </summary>
    [DataMember(Name = "name", IsRequired = true)]
    public string Name { get; protected set; }

    /// <summary>
    /// SRL Abbrevation of the game, used globabally for indexing
    /// </summary>
    [DataMember(Name = "abbrev", IsRequired = true)]
    public string Abbrevation { get; protected set; }

    /// <summary>
    /// The games popularity
    /// </summary>
    [DataMember(Name = "popularity", IsRequired = true)]
    public int Popularity { get; protected set; }

    /// <summary>
    /// The games popularity rank
    /// </summary>
    [DataMember(Name = "popularityrank", IsRequired = true)]
    public int PopularityRank { get; protected set; }

    /// <summary>
    /// The game specific rules
    /// </summary>
    public string Rules { get; internal set; }
  }
}
