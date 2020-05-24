using System;
using NUnit.Framework;
using SRLApiClient;

namespace Tests
{
  public class BaseTest
  {
    protected SRLClient _client;

    [OneTimeSetUp]
    public void SetUp()
      => _client = new SRLClient();

    [OneTimeTearDown]
    public void TearDown()
    {
      try
      {
        _client.Dispose();
      }
      catch (ObjectDisposedException) { }
    }
  }
}
