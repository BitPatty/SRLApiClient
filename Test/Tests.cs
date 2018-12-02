using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using SRLApiClient;
using SRLApiClient.Endpoints.Players;
using SRLApiClient.Endpoints.PastRaces;
using SRLApiClient.Endpoints.Races;

namespace Tests
{
  public class Tests
  {
    SRLClient _client;

    [SetUp]
    public void Setup()
    {
      _client = new SRLClient();
    }

    [Test]
    public void TestCountriesEndpoint()
    {
      ReadOnlyCollection<string> countries = _client.Countries.Get();
      Assert.Contains("Switzerland", countries);
    }

    [Test]
    public void TestRacesPlayers()
    {
      ReadOnlyCollection<Race> races = _client.Races.GetActive();
      Assert.GreaterOrEqual(races.Count, 0);
      Race tRace = races.FirstOrDefault(r => r.Entrants.Count > 0);
      Assert.IsNotNull(tRace);
      Player tPlayer = _client.Players.Get(tRace.Entrants[0].Name);
      Assert.AreEqual(tRace.Entrants[0].Twitch, tPlayer.Channel);
    }

    [Test]
    public void TestPastRacesPlayers()
    {
      PastRace pastRace = _client.PastRaces.Get("239545");
      Assert.Equals(pastRace.Results.Count, 2);
      Player tPlayer = _client.Players.Get(pastRace.Results[0].PlayerName);
      Assert.IsNotNull(tPlayer);
      Assert.Equals(tPlayer.Name, pastRace.Results[0].PlayerName);
    }
  }
}
