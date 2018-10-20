using SRLApiClient.Endpoints;
using SRLApiClient.Endpoints.Players;
using System.Runtime.Serialization;

namespace SRLApiClient
{
  /// <summary>
  /// User account used for authenticated <see cref="SRLClient"/> requests
  /// </summary>
  public class SRLUser
  {
    private SRLClient _srlClient { get; set; }

    /// <summary>
    /// The users SRL id
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// The username
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The user role / permission level
    /// </summary>
    public UserRole Role { get; private set; } = UserRole.Unknown;

    /// <summary>
    /// <see cref="Endpoints.Players.Player"/> object returned from the <see cref="Endpoints.Players.PlayersClient"/>
    /// </summary>
    public Player Player { get; private set; }

    /// <summary>
    /// Creates a new instance of <see cref="SRLUser"/>
    /// </summary>
    /// <param name="baseClient">The base client used for requests</param>
    public SRLUser(SRLClient baseClient)
    {
      _srlClient = baseClient;
    }

    /// <summary>
    /// Verifies that the <see cref="SRLUser"/> is an authenticated user
    /// </summary>
    /// <returns>Returns true if the <see cref="SRLUser"/> account could be verified</returns>
    public bool Verify()
    {
      if (_srlClient.Get("/token", out Token t) && t.Role > UserRole.Anon && _srlClient.Get("/players/" + t.Name, out Player p))
      {
        Name = t.Name;
        Role = t.Role;
        Player = p;
        return true;
      }
      return false;
    }

    /// <summary>
    /// Modifies the channel used for streams
    /// </summary>
    /// <param name="s">The users channel name</param>
    /// <returns>Returns true on success</returns>
    public bool SetChannel(string s) => new PlayersClient(_srlClient).Edit(Name, channel: s);

    /// <summary>
    /// Modifies the users twitter account
    /// </summary>
    /// <param name="s">The users twitter name</param>
    /// <returns>Returns true on success</returns>
    public bool SetTwitter(string s) => new PlayersClient(_srlClient).Edit(Name, twitter: s);

    /// <summary>
    /// Modifies the users youtube account
    /// </summary>
    /// <param name="s">The users youtube name</param>
    /// <returns>Returns true on success</returns>
    public bool SetYoutube(string s) => new PlayersClient(_srlClient).Edit(Name, youtube: s);

    /// <summary>
    /// Modifies the capitalization of the users username
    /// </summary>
    /// <param name="s">The username with the new capitalizatione</param>
    /// <returns>Returns true on success</returns>
    public bool SetCapitalization(string s) => new PlayersClient(_srlClient).Edit(Name, casename: s);

    /// <summary>
    /// Modifies the users streaming API
    /// </summary>
    /// <param name="s">The API used for streams</param>
    /// <returns>Returns true on success</returns>
    public bool SetApi(StreamApi s) => new PlayersClient(_srlClient).Edit(Name, api: s);

    /// <summary>
    /// Modifies the users country
    /// </summary>
    /// <param name="s">The users country name. To get a list of valid countries use <see cref="SRLClient.Countries"/></param>
    /// <returns>Returns true on success</returns>
    public bool SetCountry(string s) => new PlayersClient(_srlClient).Edit(Name, country: s);

    [DataContract, KnownType(typeof(SRLDataType))]
    private class Token : SRLDataType
    {
      [DataMember(Name = "user", IsRequired = true)]
      public string Name { get; private set; }

      [DataMember(Name = "role", IsRequired = true)]
      private string _role { get; set; }

      public UserRole Role
      {
        get
        {
          switch (_role)
          {
            case "anon": return UserRole.Anon;
            case "user": return UserRole.User;
            case "voice": return UserRole.Voice;
            case "halfop": return UserRole.Halfop;
            case "op": return UserRole.Op;
            case "admin": return UserRole.Admin;
            default: return UserRole.Unknown;
          }
        }
      }
    }
  }

  /// <summary>
  /// The users permission level
  /// </summary>
  public enum UserRole
  {
    /// <summary>
    /// Fallback value
    /// </summary>
    Unknown,

    /// <summary>
    /// Anonymous permissions
    /// </summary>
    Anon,

    /// <summary>
    /// User permissions
    /// </summary>
    User,

    /// <summary>
    /// Voice permissions
    /// </summary>
    Voice,

    /// <summary>
    /// Half Operator permissions
    /// </summary>
    Halfop,

    /// <summary>
    /// Operator permissions
    /// </summary>
    Op,

    /// <summary>
    /// Admin permissions
    /// </summary>
    Admin
  }
}
