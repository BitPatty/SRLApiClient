using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;

namespace SRLApiClient.Endpoints.Countries
{
  public class CountriesClient : SRLEndpoint
  {
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
    /// Fetches the list of available countries on SRL
    /// </summary>
    /// <returns>List of countries</returns>
    public ReadOnlyCollection<string> Get()
    {
      if (SrlClient.Get(BasePath, out Countries countries)) return countries.List.AsReadOnly();
      return null;
    }
  }
}
