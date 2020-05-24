using NUnit.Framework;
using SRLApiClient.Endpoints.Leaderboards;
using SRLApiClient.Extensions;

namespace Tests
{
  public class LeaderboardExtensions : BaseTest
  {
    [Test]
    [Category(nameof(LeaderboardExtensions))]
    [Parallelizable]
    public void ContainsPlayer()
    {
      Leaderboard board = _client.Leaderboards.Get("sms");
      Assert.True(board.ContainsPlayer("psychonauter"));
      Assert.False(board.ContainsPlayer("nonExistingPlayer"));
    }

    [Test]
    [Category(nameof(LeaderboardExtensions))]
    [Parallelizable]
    public void FindPlayer()
    {
      Leaderboard board = _client.Leaderboards.Get("sms");
      Leader l = board.FindPlayer("psychonauter");
      Assert.IsNotNull(l);
      Assert.AreEqual(l.Name.ToLower(), "psychonauter");

      l = board.FindPlayer("nonExistingPlayer");
      Assert.Null(l);
    }
  }
}
