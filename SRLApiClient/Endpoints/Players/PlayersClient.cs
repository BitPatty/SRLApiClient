using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Players
{
  public class PlayersClient : SRLEndpoint
  {
    public PlayersClient(SRLClient baseClient) : base("/players", baseClient) { }

    /// <summary>
    /// Fetches a single player
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
    /// Search for a player
    /// </summary>
    /// <param name="name">Name of the player</param>
    /// <returns>List of players matching the queried name</returns>
    public ReadOnlyCollection<Player> Search(string name)
    {
      if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name), "Parameter can't be empty");

      if (SrlClient.Get(BasePath + "?search=" + name.ToLower(), out PlayerSearch ps)) return ps.Players.AsReadOnly();
      return null;
    }

    /// <summary>
    /// Edit a players profile
    /// </summary>
    /// <permission cref="UserRole.User">You can only edit your own channel unless you're racebot</permission>
    /// <param name="playerName">Player name (used for indexing)</param>
    /// <param name="youtube">New youtube channel</param>
    /// <param name="twitter">New twitter name</param>
    /// <param name="channel">New streaming channel</param>
    /// <param name="api">New Streaming API</param>
    /// <param name="country">New Country</param>
    /// <param name="casename">New name capitalization</param>
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
        if (casename != null && casename.Equals(playerName, StringComparison.OrdinalIgnoreCase)) dict.Add("casename", casename);
        if (dict.Count > 0) return SrlClient.Put(BasePath + "/" + playerName, dict);
      }

      return false;
    }

    /// <summary>
    /// Fetches a single player
    /// </summary>
    /// <param name="name">the players name</param>
    /// <returns>Returns the player or null</returns>
    public Player this[string name] => Get(name);
  }
}
