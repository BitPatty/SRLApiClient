using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.PastRaces
{
  /// <summary>
  /// Result object
  /// </summary>
  [DataContract, KnownType(typeof(SRLData))]
  public class Result : SRLData
  {
    /// <summary>
    /// The race id associated with the result
    /// </summary>
    [DataMember(Name = "race", IsRequired = true)]
    public int Race { get; protected set; }

    /// <summary>
    /// The players placement in the race
    /// </summary>
    [DataMember(Name = "place", IsRequired = true)]
    public int Place { get; protected set; }

    /// <summary>
    /// The players name
    /// </summary>
    [DataMember(Name = "player", IsRequired = true)]
    public string PlayerName { get; protected set; }

    /// <summary>
    /// The players final time in seconds
    /// </summary>
    [DataMember(Name = "time", IsRequired = true)]
    public int Time { get; protected set; }

    /// <summary>
    /// The players comment on the result
    /// </summary>
    [DataMember(Name = "message", IsRequired = true)]
    public string Comment { get; protected set; }

    /// <summary>
    /// The players old trueskill
    /// </summary>
    [DataMember(Name = "oldtrueskill", IsRequired = true)]
    public int OldTrueSkill { get; protected set; }

    /// <summary>
    /// The players new trueskill
    /// </summary>
    [DataMember(Name = "newtrueskill", IsRequired = true)]
    public int NewTrueSkill { get; protected set; }

    /// <summary>
    /// The players trueskill change
    /// </summary>
    [DataMember(Name = "trueskillchange", IsRequired = true)]
    public int TrueSkillChange { get; protected set; }

    /// <summary>
    /// The players old trueskill (seasons only)
    /// </summary>
    [DataMember(Name = "oldseasontrueskill", IsRequired = true)]
    public int OldSeasonTrueSkill { get; protected set; }

    /// <summary>
    /// The players new trueskill (seasons only)
    /// </summary>
    [DataMember(Name = "newseasontrueskill", IsRequired = true)]
    public int NewSeasonTrueSkill { get; protected set; }

    /// <summary>
    /// The players trueskill change (seasons only)
    /// </summary>
    [DataMember(Name = "seasontrueskillchange", IsRequired = true)]
    public int SeasonTrueSkillChange { get; protected set; }
  }
}
