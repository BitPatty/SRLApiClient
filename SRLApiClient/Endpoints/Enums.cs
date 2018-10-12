namespace SRLApiClient.Endpoints
{
  /// <summary>
  /// The current state of an entrant
  /// </summary>
  public enum EntrantState
  {
    /// <summary>
    /// Fallback value
    /// </summary>
    Unknown,

    /// <summary>
    /// The entrant joined the race but hasn't readied up yet
    /// </summary>
    Entered,

    /// <summary>
    /// The entrant is ready
    /// </summary>
    Ready,

    /// <summary>
    /// The entrant has finished the race
    /// </summary>
    Done,

    /// <summary>
    /// The entrant quit the race
    /// </summary>
    Forfeit,

    /// <summary>
    /// The entrant got disqualified
    /// </summary>
    Disqualified
  }

  /// <summary>
  /// The current state of the race
  /// </summary>
  public enum RaceState
  {
    /// <summary>
    /// Fallback value
    /// </summary>
    Unknown,

    /// <summary>
    /// The race is open for players
    /// </summary>
    EntryOpen,

    /// <summary>
    /// The race is closed but the race hasn't started yet
    /// (This is where the 10 second countdown happens)
    /// </summary>
    EntryClosed,

    /// <summary>
    /// The race is in progress
    /// </summary>
    InProgress,

    /// <summary>
    /// The race has finished but hasn't been recorded yet
    /// </summary>
    Finished,

    /// <summary>
    /// The race has been recorded
    /// </summary>
    Over
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
