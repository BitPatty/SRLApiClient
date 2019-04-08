using System;
using System.Linq;
using SRLApiClient.Endpoints.Leaderboards;

namespace SRLApiClient.Extensions
{
  /// <summary>
  /// Extensions for the <see cref="Leaderboard"/> type
  /// </summary>
  public static class LeaderboardExtensions
  {
    /// <summary>
    /// Checks if a players name is on the leaderboard
    /// </summary>
    /// <param name="leaderboard">The leaderboard to check</param>
    /// <param name="playerName">The players name</param>
    /// <returns>Returns true if a match was found</returns>
    public static bool ContainsPlayer(this Leaderboard leaderboard, string playerName)
      => leaderboard.FindPlayer(playerName) != null;

    /// <summary>
    /// Returns the first leader on the leaderboard matching <paramref name="playerName"/>
    /// </summary>
    /// <param name="leaderboard">The leaderboard to check</param>
    /// <param name="playerName">The players name</param>
    /// <returns>Returns the leader</returns>
    public static Leader FindPlayer(this Leaderboard leaderboard, string playerName)
      => leaderboard.Leaders.FirstOrDefault(l => l.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase));
  }
}
