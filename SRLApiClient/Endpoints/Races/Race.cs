using SRLApiClient.Endpoints.Games;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Races
{
  [DataContract, KnownType(typeof(SRLDataType))]
  public class Race : SRLDataType
  {
    /// <summary>
    /// The race id
    /// </summary>
    [DataMember(Name = "id", IsRequired = true)]
    public string Id { get; protected set; }

    /// <summary>
    /// The game associated with the race
    /// </summary>
    [DataMember(Name = "game", IsRequired = true)]
    public Game Game { get; protected set; }

    /// <summary>
    /// The races goal
    /// </summary>
    [DataMember(Name = "goal", IsRequired = true)]
    public string Goal { get; protected set; }

    /// <summary>
    /// The final time of the race
    /// </summary>
    [DataMember(Name = "time", IsRequired = true)]
    public int Time { get; protected set; }

    /// <summary>
    /// The races current state
    /// </summary>
    public RaceState State
    {
      get
      {
        switch (state)
        {
          case 1: return RaceState.EntryOpen;
          case 2: return RaceState.EntryClosed;
          case 3: return RaceState.InProgress;
          case 4: return RaceState.Finished;
          case 5: return RaceState.Over;
          default: return RaceState.Unknown;
        }
      }
    }

    [DataMember(Name = "state", IsRequired = true)]
    protected int state { get; set; }

    /// <summary>
    /// The races current state text
    /// </summary>
    [DataMember(Name = "statetext", IsRequired = true)]
    public string StateText { get; protected set; }

    /// <summary>
    /// The file name used for the race
    /// </summary>
    [DataMember(Name = "filename", IsRequired = true)]
    public string FileName { get; protected set; }

    /// <summary>
    /// The number of entrants
    /// </summary>
    [DataMember(Name = "numentrants", IsRequired = true)]
    public int NumEntrants { get; protected set; }

    /// <summary>
    /// List of race entrants
    /// </summary>
    [DataMember(Name = "entrants", IsRequired = true)]
    protected List<Entrant> entrants { get; set; }
    public ReadOnlyCollection<Entrant> Entrants => entrants.AsReadOnly();
  }
}
