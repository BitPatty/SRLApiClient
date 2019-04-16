using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SRLApiClient.Exceptions;

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
    /// Gets a game synchronously
    /// </summary>
    /// <param name="abbrev">The games abbreviation</param>
    /// <returns>Returns the game</returns>
    public Game Get(string abbrev)
    {
      try
      {
        return SrlClient.Get<Game>($"{BasePath}/{abbrev.ToLower()}");
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets a game asynchronously
    /// </summary>
    /// <param name="abbrev">The games abbreviation</param>
    /// <returns>Returns the game</returns>
    public async Task<Game> GetAsync(string abbrev)
    {
      try
      {
        return await SrlClient.GetAsync<Game>($"{BasePath}/{abbrev.ToLower()}").ConfigureAwait(false);
      }
      catch (SRLParseException)
      {
        return null;
      }
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
    /// Searches for a game synchronously
    /// </summary>
    /// <param name="name">The games name or abbreviation to search for</param>
    /// <returns>Returns a list of games matching the search query</returns>
    public ReadOnlyCollection<Game> Search(string name)
    {
      if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name), "Parameter can't be empty");
      try
      {
        GameSearch gs = SrlClient.Get<GameSearch>($"{BasePath}?search={name.ToLower()}");
        return gs.Games.AsReadOnly();
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Searches for a game asynchronously
    /// </summary>
    /// <param name="name">The games name or abbreviation to search for</param>
    /// <returns>Returns a list of games matching the search query</returns>
    public async Task<ReadOnlyCollection<Game>> SearchAsync(string name)
    {
      if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name), "Parameter can't be empty");

      try
      {
        GameSearch gs = await SrlClient.GetAsync<GameSearch>($"{BasePath}?search={name.ToLower()}").ConfigureAwait(false);
        return gs.Games.AsReadOnly();
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Rules object
    /// </summary>
    [DataContract]
    private sealed class GameRules : SRLDataType
    {
      /// <summary>
      /// The games rules defined on SRL (HTML format)
      /// </summary>
      [DataMember(Name = "rules", IsRequired = true)]
      public string Rules { get; private set; }
    }

    /// <summary>
    /// Gets the rules for a game synchronously
    /// </summary>
    /// <param name="g">The game</param>
    /// <returns>Returns the rules string</returns>
    public string GetRules(Game g) => GetRules(g?.Abbreviation);

    /// <summary>
    /// Gets the rules for a game synchronously
    /// </summary>
    /// <param name="gameAbbreviation">The games abbreviation</param>
    /// <returns>Returns the rules string</returns>
    public string GetRules(string gameAbbreviation)
    {
      if (String.IsNullOrWhiteSpace(gameAbbreviation)) throw new ArgumentException(nameof(gameAbbreviation), "Parameter can't be empty");

      try
      {
        return SrlClient.Get<GameRules>($"/rules/{gameAbbreviation}").Rules;
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets the rules for a game asynchronously
    /// </summary>
    /// <param name="g">The game</param>
    /// <returns>Returns the rules string</returns>
    public async Task<string> GetRulesAsync(Game g) => await GetRulesAsync(g?.Abbreviation).ConfigureAwait(false);

    /// <summary>
    /// Gets the rules for a game asynchronously
    /// </summary>
    /// <param name="gameAbbreviation">The games abbreviation</param>
    /// <returns>Returns the rules string</returns>
    public async Task<string> GetRulesAsync(string gameAbbreviation)
    {
      if (String.IsNullOrWhiteSpace(gameAbbreviation)) throw new ArgumentException(nameof(gameAbbreviation), "Parameter can't be empty");

      try
      {
        return (await SrlClient.GetAsync<GameRules>($"/rules/{gameAbbreviation}").ConfigureAwait(false)).Rules;
      }
      catch (SRLParseException)
      {
        return null;
      }
    }
  }
}
