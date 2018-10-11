using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Races
{
  public class RacesClient : SRLEndpoint
  {
    public RacesClient(SRLClient baseClient) : base("/races", baseClient) { }

    /// <summary>
    /// Fetches a single race
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the race or null</returns>
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
    /// <permission cref="UserRole.Voice">Requires at least 'voice' permission</permission>
    /// <param name="game"></param>
    /// <returns>Returns the race on success</returns>
    /// <exception cref="SRLApiException"></exception>
    public Race Create(string game)
    {
      if (string.IsNullOrWhiteSpace(game)) throw new SRLApiException(nameof(game), "Parameter can't be empty");
      if (SrlClient.User != null && SrlClient.User.Role < UserRole.Voice) throw new SRLApiException("Missing permission");
      if (SrlClient.Post(BasePath, new Dictionary<string, string>() { { "game", game } }, out HttpResponseMessage response))
      {
        if (SrlClient.DeSerialize(response.Content.ReadAsStreamAsync().Result, out Race r)) return r;
        else throw new SRLApiException("Failed to Deserialize Response");
      }
      else throw new SRLApiException("Failed to create Race");
    }

    /// <summary>
    /// Fetches a single race
    /// </summary>
    /// <param name="raceId">The race id</param>
    /// <returns>Returns the race or null</returns>
    public Race this[string raceId] => Get(raceId);
  }
}
