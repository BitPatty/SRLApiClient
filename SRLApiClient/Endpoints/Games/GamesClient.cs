using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Games
{
  /// <summary>
  /// A client to perform requests on the /games endpoint
  /// </summary>
  public class GamesClient : SRLEndpoint
  {
    /// <summary>
    /// Creates a new client to perform requests on the /games endpoint
    /// </summary>
    /// <param name="baseClient">The <see cref="SRLClient"/> used to perform requests</param>
    public GamesClient(SRLClient baseClient) : base("/games", baseClient) { }

    [DataContract, KnownType(typeof(SRLDataType))]
    private sealed class Games : SRLDataType
    {
      [DataMember(Name = "games", IsRequired = true)]
      public List<Game> _games { get; private set; }
    }

    /// <summary>
    /// Gets a game from SRL
    /// </summary>
    /// <param name="abbrev">The games abbrevation</param>
    /// <returns>Returns the game or null</returns>
    public Game Get(string abbrev)
    {
      if (SrlClient.Get(BasePath + "/" + abbrev.ToLower(), out Game g) && SrlClient.Get("/rules/" + abbrev.ToLower(), out GameRules rules))
      {
        g.Rules = rules.Rules;
        return g;
      }

      return null;
    }

    [DataContract]
    private sealed class GameSearch : SRLDataType
    {
      [DataMember(Name = "count", IsRequired = true)]
      public int Count { get; private set; }

      [DataMember(Name = "games", IsRequired = true)]
      public List<Game> Games { get; private set; }
    }

    /// <summary>
    /// Searches for a game on SRL
    /// </summary>
    /// <param name="name">The games name or abbrevation to search for</param>
    /// <returns>Returns a list of games matching the search query</returns>
    public ReadOnlyCollection<Game> Search(string name)
    {
      if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name), "Parameter can't be empty");
      if (SrlClient.Get(BasePath + "?search=" + name.ToLower(), out GameSearch gs)) return gs.Games.AsReadOnly();
      return null;
    }

    /// <summary>
    /// Gets a game from SRL
    /// </summary>
    /// <param name="abbrev">The games abbrevation</param>
    /// <returns>Returns the game or null</returns>
    public Game this[string abbrev] => Get(abbrev);
  }
}
