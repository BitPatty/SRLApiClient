namespace SRLApiClient.Endpoints.PastRaces
{
  public class PastRacesClient : SRLEndpoint
  {
    public PastRacesClient(SRLClient baseClient) : base("/pastraces", baseClient) { }

    /// <summary>
    /// Fetches a single past race
    /// </summary>
    /// <param name="raceId">Id of the race</param>
    /// <returns></returns>
    public PastRace Get(string raceId)
    {
      SrlClient.Get(BasePath + "/" + raceId.ToLower(), out PastRace r);
      if (r != null)
      {
        SrlClient.Get("/rules/" + r.Game.Abbrevation, out GameRules gr);
        r.Game.Rules = gr.Rules;
      }

      return r;
    }

    /// <summary>
    /// Fetches a single past race
    /// </summary>
    /// <param name="raceId">The race id</param>
    /// <returns>Returns the past race or null</returns>
    public PastRace this[string raceId] => Get(raceId);

    /// <summary>
    /// Fetches a single past race
    /// </summary>
    /// <param name="raceId">The race id</param>
    /// <returns>Returns the past race or null</returns>
    public PastRace this[int raceId] => Get(raceId.ToString());
  }
}
