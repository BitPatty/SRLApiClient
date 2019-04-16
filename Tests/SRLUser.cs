using System;
using NUnit.Framework;
using SRLApiClient;

namespace Tests
{
  public class SRLUser
  {
    internal SRLClient _client { get; set; }
    internal string _username => Environment.GetEnvironmentVariable("SRLUsername");
    internal string _password => Environment.GetEnvironmentVariable("SRLPassword");

    [SetUp]
    public void Setup()
    {
      _client = new SRLClient();
      _client.Authenticate(_username, _password);
    }

    [Test]
    [Category("Authentication")]
    public void Authenticate()
    {
      Assert.IsTrue(_client.IsAuthenticated);
      Assert.AreEqual(_client.User.Name?.ToLower(), _username.ToLower());
    }

    [Test]
    [Category("Authentication")]
    public void ChangeCapitalization()
    {
      Assert.IsTrue(_client.IsAuthenticated);
      Assert.NotNull(_client.User.Name);

      string displayName = _client.Players.Get(_client.User.Name)?.Name;
      Assert.NotNull(displayName);

      if (_username.ToUpper().Equals(displayName))
      {
        Assert.IsTrue(_client.User.SetCapitalization(_username.ToLower()));

        displayName = _client.Players.Get(_client.User.Name)?.Name;

        Assert.NotNull(displayName);
        Assert.AreEqual(_username.ToLower(), displayName);
      }
      else
      {
        Assert.IsTrue(_client.User.SetCapitalization(_username.ToUpper()));

        displayName = _client.Players.Get(_client.User.Name)?.Name;

        Assert.NotNull(displayName);
        Assert.AreEqual(_username.ToUpper(), displayName);
      }
    }
  }
}
