using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SRLApiClient.Endpoints.PastRaces
{
  /// <summary>
  /// A client to perform requests on the /pastraces endpoint
  /// </summary>
  public class PastRacesClient : SRLEndpoint
  {
    /// <summary>
    /// Creates a new client to perform requests on the /pastraces endpoint
    /// </summary>
    /// <param name="baseClient">The <see cref="SRLClient"/> used to perform requests</param>
    public PastRacesClient(SRLClient baseClient) : base("/pastraces", baseClient) { }

    [DataContract]
    private sealed class PastRacesCollection : SRLData
    {
      [DataMember(Name = "count", IsRequired = true)]
      public int Count { get; private set; }

      [DataMember(Name = "pastraces", IsRequired = true)]
      public List<PastRace> Races { get; private set; }
    }

    /// <summary>
    /// Gets a collection of past races synchronously
    /// </summary>
    /// <param name="playerName">The players name</param>
    /// <param name="gameAbbreviation">The games abbreviation</param>
    /// <param name="page">The page number</param>
    /// <param name="pageSize">The page size</param>
    /// <returns>Returns the race collection</returns>
    public ReadOnlyCollection<PastRace> Get(string playerName = null, string gameAbbreviation = null, int page = 1, int pageSize = 20)
    {
      if (page < 1) throw new ArgumentException(nameof(page), "Parameter must be 1 or greater");
      if (pageSize < 1) throw new ArgumentException(nameof(pageSize), "Parameter must be 1 or greater");

      string playerFilter = !string.IsNullOrWhiteSpace(playerName) ? $"&player={playerName}" : "";
      string gameFilter = !string.IsNullOrWhiteSpace(gameAbbreviation) ? $"&game={gameAbbreviation}" : "";

      return SrlClient.Get<PastRacesCollection>($"{BasePath}?page={page}&pageSize={pageSize}{playerFilter}{gameFilter}")?.Races?.AsReadOnly();
    }

    /// <summary>
    /// Gets a collection of past races asynchronously
    /// </summary>
    /// <param name="playerName">The players name</param>
    /// <param name="gameAbbreviation">The games abbreviation</param>
    /// <param name="page">The page number</param>
    /// <param name="pageSize">The page size</param>
    /// <returns>Returns the race collection</returns>
    public async Task<ReadOnlyCollection<PastRace>> GetAsync(string playerName = null, string gameAbbreviation = null, int page = 1, int pageSize = 20)
    {
      if (page < 1) throw new ArgumentException(nameof(page), "Parameter must be 1 or greater");
      if (pageSize < 1) throw new ArgumentException(nameof(pageSize), "Parameter must be 1 or greater");

      string playerFilter = !string.IsNullOrWhiteSpace(playerName) ? $"&player={playerName}" : "";
      string gameFilter = !string.IsNullOrWhiteSpace(gameAbbreviation) ? $"&game={gameAbbreviation}" : "";

      return (await SrlClient.GetAsync<PastRacesCollection>($"{BasePath}?page={page}&pageSize={pageSize}{playerFilter}{gameFilter}").ConfigureAwait(false))?.Races?.AsReadOnly();
    }

    /// <summary>
    /// Gets a single past race synchronously
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the past race</returns>
    public PastRace Get(string raceId)
    {
      if (string.IsNullOrWhiteSpace(raceId)) throw new ArgumentException(nameof(raceId), "Parameter can't be empty");
      return SrlClient.Get<PastRacesCollection>($"{BasePath}/{raceId.ToLower()}")?.Races?.FirstOrDefault();
    }

    /// <summary>
    /// Gets a single past race asynchronously
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the past race</returns>
    public async Task<PastRace> GetAsync(string raceId)
    {
      if (string.IsNullOrWhiteSpace(raceId)) throw new ArgumentException(nameof(raceId), "Parameter can't be empty");
      return (await SrlClient.GetAsync<PastRacesCollection>($"{BasePath}/{raceId.ToLower()}").ConfigureAwait(false))?.Races?.FirstOrDefault();
    }
  }
}
