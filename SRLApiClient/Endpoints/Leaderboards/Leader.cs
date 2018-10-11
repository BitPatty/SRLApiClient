using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Leaderboards
{

  [DataContract, KnownType(typeof(SRLDataType))]
  public class Leader : SRLDataType
  {
    /// <summary>
    /// Player Name
    /// </summary>
    [DataMember(Name = "name", IsRequired = true)]
    public string Name { get; protected set; }

    /// <summary>
    /// The players trueskill value for the game
    /// </summary>
    [DataMember(Name = "trueskill", IsRequired = true)]
    public float TrueSkill { get; protected set; }

    /// <summary>
    /// The players rank on the leaderboard
    /// </summary>
    [DataMember(Name = "rank", IsRequired = true)]
    public int Rank { get; protected set; }
  }
}
