using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SRLApiClient.Exceptions;

namespace SRLApiClient.Endpoints.Countries
{
  /// <summary>
  /// A client to perform requests on the /country endpoint
  /// </summary>
  public class CountriesClient : SRLEndpoint
  {
    /// <summary>
    /// Creates a new client to perform requests on the /country endpoint
    /// </summary>
    /// <param name="baseClient">The <see cref="SRLClient"/> used to perform requests</param>
    public CountriesClient(SRLClient baseClient) : base("/country", baseClient) { }

    [DataContract, KnownType(typeof(SRLData))]
    private sealed class Countries : SRLData
    {
      [DataMember(Name = "count", IsRequired = true)]
      public int Count { get; private set; }

      [DataMember(Name = "countries", IsRequired = true)]
      public List<string> List { get; private set; }
    }

    /// <summary>
    /// Gets the list of available countries synchronously
    /// </summary>
    /// <returns>Returns the list of countries</returns>
    public ReadOnlyCollection<string> Get() 
      => SrlClient.Get<Countries>(BasePath).List.AsReadOnly();

    /// <summary>
    /// Gets the list of available countries synchronously
    /// </summary>
    /// <returns>Returns the list of countries</returns>
    public async Task<ReadOnlyCollection<string>> GetAsync() 
      => (await SrlClient.GetAsync<Countries>(BasePath).ConfigureAwait(false)).List.AsReadOnly();
  }
}
