using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Games
{
  public class GamesClient : SRLEndpoint
  {
    public GamesClient(SRLClient baseClient) : base("/games", baseClient) { }

    [DataContract, KnownType(typeof(SRLDataType))]
    private sealed class Games : SRLDataType
    {
      [DataMember(Name = "games", IsRequired = true)]
      public List<Game> _games { get; private set; }
    }

    /// <summary>
    /// Fetch a game from SRL
    /// </summary>
    /// <param name="abbrev">The games abbrevation</param>
    /// <returns>Returns the Game or null</returns>
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
    /// Search for a game on SRL
    /// </summary>
    /// <param name="name">Name or abbrevation to search for</param>
    /// <returns>Returns a list of games matching the search query</returns>
    public ReadOnlyCollection<Game> Search(string name)
    {
      if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name), "Parameter can't be empty");
      if (SrlClient.Get(BasePath + "?search=" + name.ToLower(), out GameSearch gs)) return gs.Games.AsReadOnly();
      return null;
    }

    /// <summary>
    /// Fetch a game from SRL
    /// </summary>
    /// <param name="abbrev">The games abbrevation</param>
    /// <returns>Returns the Game or null</returns>
    public Game this[string abbrev] => Get(abbrev);
  }
}
