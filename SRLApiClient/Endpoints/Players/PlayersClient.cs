using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Players
{
  /// <summary>
  /// A client to perform requests on the /players endpoint
  /// </summary>
  public class PlayersClient : SRLEndpoint
  {
    /// <summary>
    /// Creates a new client to perform requests on the /players endpoint
    /// </summary>
    /// <param name="baseClient">The <see cref="SRLClient"/> used to perform requests</param>
    public PlayersClient(SRLClient baseClient) : base("/players", baseClient) { }

    /// <summary>
    /// Gets a single player
    /// </summary>
    /// <param name="name">The players name</param>
    /// <returns>The player or null</returns>
    public Player Get(string name)
    {
      SrlClient.Get(BasePath + "/" + name.ToLower(), out Player p);
      return p;
    }

    [DataContract]
    private sealed class PlayerSearch : SRLDataType
    {
      [DataMember(Name = "count", IsRequired = true)]
      public int Count { get; private set; }

      [DataMember(Name = "players", IsRequired = true)]
      public List<Player> Players { get; private set; }
    }

    /// <summary>
    /// Searches for a player
    /// </summary>
    /// <param name="name">The players name</param>
    /// <returns>List of players matching the queried name</returns>
    public ReadOnlyCollection<Player> Search(string name)
    {
      if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name), "Parameter can't be empty");

      if (SrlClient.Get(BasePath + "?search=" + name.ToLower(), out PlayerSearch ps)) return ps.Players.AsReadOnly();
      return null;
    }

    /// <summary>
    /// Edits a players profile
    /// </summary>
    /// <permission cref="UserRole.User">Editing a profile requires the <see cref="SRLClient"/> to be authorized
    /// with the appropriate account (or the racebot account)</permission>
    /// <param name="playerName">The players name</param>
    /// <param name="youtube">The new youtube channel</param>
    /// <param name="twitter">The new twitter name</param>
    /// <param name="channel">The new streaming channel</param>
    /// <param name="api">The new Streaming API</param>
    /// <param name="country">The new Country</param>
    /// <param name="casename">The new name capitalization</param>
    /// <returns>Returns true on success</returns>
    public bool Edit(string playerName, string youtube = null, string twitter = null, string channel = null, StreamApi api = StreamApi.Unknown, string country = null, string casename = null)
    {
      Player p = Get(playerName);
      if (p != null && SrlClient.User.Role >= UserRole.User && (SrlClient.User.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase) || SrlClient.User.Name.Equals("racebot", StringComparison.OrdinalIgnoreCase)))
      {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        if (youtube != null) dict.Add("youtube", p.Youtube);
        if (twitter != null) dict.Add("twitter", p.Twitter);
        if (channel != null) dict.Add("channel", p.Channel);
        if (api != StreamApi.Unknown) dict.Add("api", api.ToString().ToLower());
        if (country != null) dict.Add("country", p.Country);
        if (casename?.Equals(playerName, StringComparison.OrdinalIgnoreCase) == true) dict.Add("casename", casename);

        if (dict.Count > 0 && SrlClient.Put(BasePath + "/" + playerName, dict))
          return SrlClient.User.Verify();
      }

      return false;
    }

    /// <summary>
    /// Gets a single player
    /// </summary>
    /// <param name="name">The players name</param>
    /// <returns>Returns the player or null</returns>
    public Player this[string name] => Get(name);
  }
}
