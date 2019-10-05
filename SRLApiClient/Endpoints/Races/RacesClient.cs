using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SRLApiClient.Exceptions;

namespace SRLApiClient.Endpoints.Races
{
  /// <summary>
  /// A client to perform requests on the /races endpoint
  /// </summary>
  public class RacesClient : SRLEndpoint
  {
    /// <summary>
    /// Creates a new client to perform requests on the /races endpoint
    /// </summary>
    /// <param name="baseClient">The <see cref="SRLClient"/> used to perform requests</param>
    public RacesClient(SRLClient baseClient) : base("/races", baseClient) { }

    /// <summary>
    /// Gets a single race synchronously
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the <see cref="Race"/></returns>
    public Race Get(string raceId)
      => SrlClient.Get<Race>($"{BasePath}/{raceId.ToLower()}");

    /// <summary>
    /// Gets a single race asynchronously
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the <see cref="Race"/></returns>
    public async Task<Race> GetAsync(string raceId)
      => await SrlClient.GetAsync<Race>($"{BasePath}/{raceId.ToLower()}").ConfigureAwait(false);

    [DataContract]
    private sealed class ActiveRaces : SRLData
    {
      [DataMember(Name = "count", IsRequired = true)]
      public int Count { get; private set; }

      [DataMember(Name = "races", IsRequired = true)]
      public List<Race> Races { get; private set; }
    }

    /// <summary>
    /// Gets the currently active races synchronously
    /// </summary>
    /// <returns>Returns a list of active races</returns>
    public ReadOnlyCollection<Race> GetActive()
      => SrlClient.Get<ActiveRaces>(BasePath)?.Races?.AsReadOnly();

    /// <summary>
    /// Gets the currently active races asynchronously
    /// </summary>
    /// <returns>Returns a list of active races</returns>
    public async Task<ReadOnlyCollection<Race>> GetActiveAsync()
      => (await SrlClient.GetAsync<ActiveRaces>(BasePath).ConfigureAwait(false))?.Races?.AsReadOnly();

    /// <summary>
    /// Gets a single race
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the race</returns>
    public Race this[string raceId] => Get(raceId);
  }
}
