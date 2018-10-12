namespace SRLApiClient.Endpoints.Leaderboards
{
  /// <summary>
  /// A client to perform requests on the /leaderboard endpoint
  /// </summary>
  public class LeaderboardsClient : SRLEndpoint
  {
    /// <summary>
    /// Creates a new client to perform requests on the /leaderboard endpoint
    /// </summary>
    /// <param name="baseClient">The <see cref="SRLClient"/> used to perform requests</param>
    public LeaderboardsClient(SRLClient baseClient) : base("/leaderboard", baseClient) { }

    /// <summary>
    /// Gets a single leaderboard
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
    /// Gets a single leaderboard
    /// </summary>
    /// <param name="gameAbbrevation">The games abbrevation</param>
    /// <returns>Returns the leaderboard or null</returns>
    public Leaderboard this[string gameAbbrevation] => Get(gameAbbrevation);
  }
}
