namespace SRLApiClient.Endpoints
{
  /// <summary>
  /// The current state of an entrant
  /// </summary>
  public enum EntrantState : uint
  {
    /// <summary>
    /// Fallback value
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The entrant joined the race but hasn't readied up yet
    /// </summary>
    Entered = 1,

    /// <summary>
    /// The entrant is ready
    /// </summary>
    Ready = 2,

    /// <summary>
    /// The entrant has finished the race
    /// </summary>
    Done = 3,

    /// <summary>
    /// The entrant quit the race
    /// </summary>
    Forfeit = 4,

    /// <summary>
    /// The entrant got disqualified
    /// </summary>
    Disqualified = 5
  }
}
