using NUnit.Framework;
using SRLApiClient;
using SRLApiClient.Endpoints.Leaderboards;
using SRLApiClient.Extensions;

namespace Tests
{
  public class LeaderboardExtensions
  {
    internal SRLClient _client { get; set; }

    [SetUp]
    public void Setup()
    {
      _client = new SRLClient();
    }

    [Test]
    public void ContainsPlayer()
    {
      Leaderboard board = _client.Leaderboards.Get("sms");
      Assert.True(board.ContainsPlayer("psychonauter"));
      Assert.False(board.ContainsPlayer("nonExistingPlayer"));
    }

    [Test]
    public void FindPlayer()
    {
      Leaderboard board = _client.Leaderboards.Get("sms");
      Leader l = board.FindPlayer("psychonauter");
      Assert.IsNotNull(l);
      Assert.AreEqual(l.Name, "psychonauter");

      l = board.FindPlayer("nonExistingPlayer");
      Assert.Null(l);
    }
  }
}
