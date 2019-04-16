namespace SRLApiClient.Endpoints
{
  /// <summary>
  /// The current state of the race
  /// </summary>
  public enum RaceState : uint
  {
    /// <summary>
    /// Fallback value
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The race is open for players
    /// </summary>
    EntryOpen = 1,

    /// <summary>
    /// The race is closed but the race hasn't started yet
    /// (This is where the 10 second countdown happens)
    /// </summary>
    EntryClosed = 2,

    /// <summary>
    /// The race is in progress
    /// </summary>
    InProgress = 3,

    /// <summary>
    /// The race has finished but hasn't been recorded yet
    /// </summary>
    Finished = 4,

    /// <summary>
    /// The race has been recorded
    /// </summary>
    Over = 5
  }
}
