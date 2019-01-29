using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

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

    /// <summary>
    /// Gets a single past race
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the past race or null</returns>
    public PastRace Get(string raceId)
    {
      if (String.IsNullOrWhiteSpace(raceId)) throw new ArgumentException(nameof(raceId), "Parameter can't be empty");

      SrlClient.Get(BasePath + "/" + raceId.ToLower(), out PlayerPastRaces r);
      PastRace pr = r?.Races[0];

      if (pr != null)
      {
        SrlClient.Get("/rules/" + pr.Game.Abbreviation, out GameRules gr);
        pr.Game.Rules = gr.Rules;
      }

      return pr;
    }

    [DataContract]
    private sealed class PlayerPastRaces : SRLDataType
    {
      [DataMember(Name = "count", IsRequired = true)]
      public int Count { get; private set; }

      [DataMember(Name = "pastraces", IsRequired = true)]
      public List<PastRace> Races { get; private set; }
    }

    /// <summary>
    /// Gets all past races from a player
    /// </summary>
    /// <param name="playerName">The players name</param>
    /// <param name="gameAbbreviation">If provided, only returns races from the specified game</param>
    /// <returns>Returns a list of past races or null</returns>
    public ReadOnlyCollection<PastRace> GetByPlayer(string playerName, string gameAbbreviation = null)
    {
      if (String.IsNullOrWhiteSpace(playerName)) throw new ArgumentException(nameof(playerName), "Parameter can't be empty");

      string path = BasePath + "?player=" + playerName;
      if (!String.IsNullOrWhiteSpace(gameAbbreviation)) path += "&game=" + gameAbbreviation;

      SrlClient.Get(path, out PlayerPastRaces ppr);
      if (ppr != null)
      {
        foreach (PastRace pr in ppr.Races)
        {
          SrlClient.Get("/rules/" + pr.Game.Abbreviation, out GameRules r);
          pr.Game.Rules = r.Rules;
        }

        return ppr.Races.AsReadOnly();
      }

      return null;
    }

    /// <summary>
    /// Gets a single past race
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the past races or null</returns>
    public PastRace this[string raceId] => Get(raceId);

    /// <summary>
    /// Gets a single past race
    /// </summary>
    /// <param name="raceId">The races id</param>
    /// <returns>Returns the past race or null</returns>
    public PastRace this[int raceId] => Get(raceId.ToString());
  }
}
