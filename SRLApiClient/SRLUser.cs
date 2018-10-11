using SRLApiClient.Endpoints;
using SRLApiClient.Endpoints.Players;
using System.Runtime.Serialization;

namespace SRLApiClient
{
  public class SRLUser
  {
    private SRLClient _srlClient { get; set; }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public UserRole Role { get; private set; } = UserRole.Unknown;
    public Player Player { get; private set; }

    public SRLUser(SRLClient baseClient)
    {
      _srlClient = baseClient;
    }

    public bool Verify()
    {
      if (_srlClient.Get("/token", out Token t) && !t.Role.Equals(UserRole.Unknown) && !t.Role.Equals(UserRole.Anon) && _srlClient.Get("/players/" + t.Name, out Player p))
      {
        Name = t.Name;
        Role = t.Role;
        Player = p;
        return true;
      }
      return false;
    }

    public bool SetChannel(string s) => new PlayersClient(_srlClient).Edit(Name, channel: s);
    public bool SetTwitter(string s) => new PlayersClient(_srlClient).Edit(Name, twitter: s);
    public bool SetYoutube(string s) => new PlayersClient(_srlClient).Edit(Name, youtube: s);
    public bool SetCapitalization(string s) => new PlayersClient(_srlClient).Edit(Name, casename: s);
    public bool SetApi(StreamApi s) => new PlayersClient(_srlClient).Edit(Name, api: s);
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

  public enum UserRole
  {
    Unknown,
    Anon,
    User,
    Voice,
    Halfop,
    Op,
    Admin
  }
}
