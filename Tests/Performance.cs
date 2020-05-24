using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NUnit.Framework;
using SRLApiClient.Endpoints.Races;

namespace Tests
{
  public class Performance : BaseTest
  {
#pragma warning disable RCS1047 // Non-asynchronous method name should not end with 'Async'.

    [Test]
    [Category(nameof(Performance))]
    [Parallelizable]
    public void RacesAsync()
    {
      DateTime startTime = DateTime.UtcNow;

      ReadOnlyCollection<Race> races = _client.Races.GetActive();

      List<Task<Race>> raceTasks = new List<Task<Race>>();

      foreach (Race race in races)
      {
        raceTasks.Add(_client.Races.GetAsync(race.Id));
      }

      Task.WaitAll(raceTasks.ToArray());

      DateTime endTime = DateTime.UtcNow;
      TestContext.WriteLine($"Duration: {endTime.Subtract(startTime).TotalSeconds}");

      raceTasks.ForEach(t => Assert.IsNotNull(t.GetAwaiter().GetResult()));
    }

#pragma warning restore RCS1047 // Non-asynchronous method name should not end with 'Async'.

    [Test]
    [Category(nameof(Performance))]
    [Parallelizable]
    public void RacesSync()
    {
      DateTime startTime = DateTime.UtcNow;

      ReadOnlyCollection<Race> races = _client.Races.GetActive();

      List<Race> fetchedRaces = new List<Race>();

      foreach (Race race in races)
      {
        fetchedRaces.Add(_client.Races.Get(race.Id));
      }

      DateTime endTime = DateTime.UtcNow;
      TestContext.WriteLine($"Duration: {endTime.Subtract(startTime).TotalSeconds}");

      fetchedRaces.ForEach(r => Assert.IsNotNull(r));
    }
  }
}
