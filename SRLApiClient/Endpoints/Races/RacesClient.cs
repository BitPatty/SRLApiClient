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
    /// Creates a race
    /// </summary>
    /// <permission cref="UserRole.Voice">Creating a race requires the <see cref="SRLClient"/> to be authorized and the
    /// associated user requires at least the 'voice' permission</permission>
    /// <param name="gameAbbrevation">The games abbrevation</param>
    /// <returns>Returns the race on success</returns>
    /// <exception cref="SRLApiException">Throws a <see cref="SRLApiException"/> on failure.</exception>
    public Race Create(string gameAbbrevation)
    {
      if (string.IsNullOrWhiteSpace(gameAbbrevation)) throw new ArgumentException(nameof(gameAbbrevation), "Parameter can't be empty");
      if (SrlClient.User != null && SrlClient.User.Role < UserRole.Voice) throw new ArgumentException("Missing permission");

      if (SrlClient.Post(BasePath, new Dictionary<string, string>() { { "game", gameAbbrevation } }, out HttpResponseMessage response))
      {
        if (SrlClient.DeSerialize(response.Content.ReadAsStreamAsync().Result, out Race r)) return r;
        else throw new SRLApiException("Failed to Deserialize Response");
      }
      else throw new SRLApiException("Failed to create Race");
    }

    /// <summary>
    /// Gets a single race
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the race or null</returns>
    public Race this[string raceId] => Get(raceId);
  }
}
