using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Races
{
  [DataContract, KnownType(typeof(SRLDataType))]
  public class Entrant : SRLDataType
  {
    /// <summary>
    /// The players name
    /// </summary>
    [DataMember(Name = "displayname", IsRequired = true)]
    public string Name { get; protected set; }

    [DataMember(Name = "place", IsRequired = true)]
    protected int place { get; set; }

    /// <summary>
    /// The players rank (-1 if not finished)
    /// </summary>
    public int Place => place >= 9990 ? -1 : place;

    /// <summary>
    /// The players time (see <see cref="State"/> if smaller than 1)
    /// </summary>
    [DataMember(Name = "time", IsRequired = true)]
    public int Time { get; protected set; }

    /// <summary>
    /// The players current state
    /// </summary>
    public EntrantState State
    {
      get
      {
        switch (Time)
        {
          case -3: return EntrantState.Ready;
          case -2: return EntrantState.Disqualified;
          case -1: return EntrantState.Forfeit;
          case 0: return EntrantState.Entered;
          default: return Time > 0 ? EntrantState.Done : EntrantState.Unknown;
        }
      }
    }

    /// <summary>
    /// The players current state text
    /// </summary>
    [DataMember(Name = "statetext", IsRequired = true)]
    public string StateText { get; protected set; }

    /// <summary>
    /// The players streaming channel
    /// </summary>
    [DataMember(Name = "twitch", IsRequired = true)]
    public string Twitch { get; protected set; }

    /// <summary>
    /// The players trueskill rank
    /// </summary>
    [DataMember(Name = "trueskill", IsRequired = true)]
    public string TrueSkill { get; protected set; }
  }
}
