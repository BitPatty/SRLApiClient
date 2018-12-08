using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Runtime.Serialization;

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
    /// Gets a single race
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the <see cref="Race"/>or null</returns>
    public Race Get(string raceId)
    {
      SrlClient.Get(BasePath + "/" + raceId.ToLower(), out Race r);
      if (r != null)
      {
        SrlClient.Get("/rules/" + r.Game.Abbrevation, out GameRules gr);
        r.Game.Rules = gr.Rules;
      }

      return r;
    }

    [DataContract]
    private sealed class ActiveRaces : SRLDataType
    {
      [DataMember(Name = "count", IsRequired = true)]
      public int Count { get; private set; }

      [DataMember(Name = "races", IsRequired = true)]
      public List<Race> Races { get; private set; }
    }

    /// <summary>
    /// Returns the currently active races
    /// </summary>
    /// <returns>Returns a list of active races</returns>
    public ReadOnlyCollection<Race> GetActive()
    {
      SrlClient.Get(BasePath, out ActiveRaces ar);
      return ar.Races.AsReadOnly();
    }

    /// <summary>
    /// Gets a single race
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the race or null</returns>
    public Race this[string raceId] => Get(raceId);
  }
}
