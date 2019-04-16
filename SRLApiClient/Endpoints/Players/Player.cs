using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Players
{
  /// <summary>
  /// Player object
  /// </summary>
  [DataContract, KnownType(typeof(SRLData))]
  public class Player : SRLData
  {
    /// <summary>
    /// The players id
    /// </summary>
    [DataMember(Name = "id", IsRequired = true)]
    public int Id { get; protected set; }

    /// <summary>
    /// The players name
    /// </summary>
    [DataMember(Name = "name", IsRequired = true)]
    public string Name { get; protected set; }

    /// <summary>
    /// The players streaming channel
    /// </summary>
    [DataMember(Name = "channel", IsRequired = false)]
    public string Channel { get; protected set; }

    /// <summary>
    /// The players twitter name
    /// </summary>
    [DataMember(Name = "twitter", IsRequired = false)]
    public string Twitter { get; protected set; }

    /// <summary>
    /// The players youtube name
    /// </summary>
    [DataMember(Name = "youtube", IsRequired = false)]
    public string Youtube { get; protected set; }

    /// <summary>
    /// The streaming API used by srl
    /// </summary>
    [DataMember(Name = "api", IsRequired = false)]
    private string _api { get; set; }

    /// <summary>
    /// The streaming API used by srl
    /// </summary>
    public StreamApi StreamApi
    {
      get
      {
        switch (_api)
        {
          case "twitch": return StreamApi.Twitch;
          case "hitbox": return StreamApi.Hitbox;
          default: return StreamApi.Unknown;
        }
      }
    }

    /// <summary>
    /// The players country
    /// </summary>
    [DataMember(Name = "country", IsRequired = false)]
    public string Country { get; protected set; }
  }
}
