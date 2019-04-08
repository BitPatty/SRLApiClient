using System.Threading.Tasks;

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
    /// Gets a single leaderboard synchronously
    /// </summary>
    /// <param name="abbrev">The games abbreviation</param>
    /// <returns>Returns the leaderboard</returns>
    public Leaderboard Get(string abbrev)
    {
      try
      {
        return SrlClient.Get<Leaderboard>($"{BasePath}/{abbrev.ToLower()}");
      }
      catch (SRLParseException)
      {
        return null;
      }
    }

    /// <summary>
    /// Gets a single leaderboard asynchronously
    /// </summary>
    /// <param name="abbrev">The games abbreviation</param>
    /// <returns>Returns the leaderboard</returns>
    public async Task<Leaderboard> GetAsync(string abbrev)
    {
      try
      {
        return await SrlClient.GetAsync<Leaderboard>($"{BasePath}/{abbrev.ToLower()}").ConfigureAwait(false);
      }
      catch (SRLParseException)
      {
        return null;
      }
    }
  }
}
