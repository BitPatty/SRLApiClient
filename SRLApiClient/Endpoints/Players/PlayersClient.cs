using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SRLApiClient.Exceptions;
using SRLApiClient.User;

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
    /// Gets a single player synchronously
    /// </summary>
    /// <param name="name">The players name</param>
    /// <returns>Returns the player</returns>
    public Player Get(string name)
    {
      try
      {
        return SrlClient.Get<Player>($"{ BasePath}/{name.ToLower()}");
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets a single player asynchronously
    /// </summary>
    /// <param name="name">The players name</param>
    /// <returns>Returns the player</returns>
    public async Task<Player> GetAsync(string name)
    {
      try
      {
        return await SrlClient.GetAsync<Player>($"{ BasePath}/{name.ToLower()}").ConfigureAwait(false);
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    [DataContract]
    private sealed class PlayerSearch : SRLData
    {
      [DataMember(Name = "count", IsRequired = true)]
      public int Count { get; private set; }

      [DataMember(Name = "players", IsRequired = true)]
      public List<Player> Players { get; private set; }
    }

    /// <summary>
    /// Searches a player synchronously
    /// </summary>
    /// <param name="name">The players name</param>
    /// <returns>List of players matching the queried name</returns>
    public ReadOnlyCollection<Player> Search(string name)
    {
      if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name), "Parameter can't be empty");

      try
      {
        return SrlClient.Get<PlayerSearch>($"{BasePath}?search={name.ToLower()}")?.Players?.AsReadOnly();
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Searches a player asynchronously
    /// </summary>
    /// <param name="name">The players name</param>
    /// <returns>List of players matching the queried name</returns>
    public async Task<ReadOnlyCollection<Player>> SearchAsync(string name)
    {
      if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name), "Parameter can't be empty");

      try
      {
        return (await SrlClient.GetAsync<PlayerSearch>($"{BasePath}?search={name.ToLower()}").ConfigureAwait(false))?.Players?.AsReadOnly();
      }
      catch (SRLParseException)
      {
        return null;
      }
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
  }
}
