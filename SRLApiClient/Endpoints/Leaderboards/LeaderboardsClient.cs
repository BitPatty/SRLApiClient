namespace SRLApiClient.Endpoints.Leaderboards
{
  public class LeaderboardsClient : SRLEndpoint
  {
    public LeaderboardsClient(SRLClient baseClient) : base("/leaderboard", baseClient) { }

    /// <summary>
    /// Fetches a single leaderboard
    /// </summary>
    /// <param name="abbrev">The games abbrevation</param>
    /// <returns>Returns the leaderboard or null</returns>
    public Leaderboard Get(string abbrev)
    {

      if (SrlClient.Get(BasePath + "/" + abbrev.ToLower(), out Leaderboard l) && SrlClient.Get("/rules/" + abbrev.ToLower(), out GameRules rules))
      {
        l.Game.Rules = rules.Rules;
        return l;
      }

      else return null;
    }

    /// <summary>
    /// Fetches a single leaderboard
    /// </summary>
    /// <param name="gameAbbrevation">The games abbrevation</param>
    /// <returns>Returns the leaderboard or null</returns>
    public Leaderboard this[string gameAbbrevation] => Get(gameAbbrevation);
  }
}
