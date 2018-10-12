using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Leaderboards
{
  /// <summary>
  /// Leader object
  /// </summary>
  [DataContract, KnownType(typeof(SRLDataType))]
  public class Leader : SRLDataType
  {
    /// <summary>
    /// The players name
    /// </summary>
    [DataMember(Name = "name", IsRequired = true)]
    public string Name { get; protected set; }

    /// <summary>
    /// The players trueskill level on the associated <see cref="Leaderboard"/>
    /// </summary>
    [DataMember(Name = "trueskill", IsRequired = true)]
    public float TrueSkill { get; protected set; }

    /// <summary>
    /// The players rank on the associated <see cref="Leaderboard"/>
    /// </summary>
    [DataMember(Name = "rank", IsRequired = true)]
    public int Rank { get; protected set; }
  }
}
