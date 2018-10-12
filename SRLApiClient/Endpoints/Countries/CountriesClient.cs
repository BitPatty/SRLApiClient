using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

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

    [DataContract, KnownType(typeof(SRLDataType))]
    private sealed class Countries : SRLDataType
    {
      [DataMember(Name = "count", IsRequired = true)]
      public int Count { get; private set; }

      [DataMember(Name = "countries", IsRequired = true)]
      public List<string> List { get; private set; }
    }

    /// <summary>
    /// Gets the list of available countries on SRL
    /// </summary>
    /// <returns>Returns the list of countries or null</returns>
    public ReadOnlyCollection<string> Get()
    {
      if (SrlClient.Get(BasePath, out Countries countries)) return countries.List.AsReadOnly();
      return null;
    }
  }
}
