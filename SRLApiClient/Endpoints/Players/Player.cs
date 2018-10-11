using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Players
{
  [DataContract, KnownType(typeof(SRLDataType))]
  public class Player : SRLDataType
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

    [DataMember(Name = "api", IsRequired = false)]
    protected string api { get; set; }

    /// <summary>
    /// The API used by srl to check for the players stream 
    /// </summary>
    public StreamApi StreamApi
    {
      get
      {
        switch (api)
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
