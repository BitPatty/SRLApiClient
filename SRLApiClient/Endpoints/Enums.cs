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

  /// <summary>
  /// The API used for fetching streams. The according
  /// channel can usually be found in the 'Channel' Property
  /// </summary>
  public enum StreamApi
  {
    /// <summary>
    /// Fallback value
    /// </summary>
    Unknown,

    /// <summary>
    /// Twitch API
    /// </summary>
    Twitch,

    /// <summary>
    /// Hitbox API
    /// </summary>
    Hitbox
  }
}
