namespace SRLApiClient.Endpoints
{
  public enum EntrantState
  {
    Unknown,
    Entered,
    Ready,
    Done,
    Forfeit,
    Disqualified
  }

  public enum RaceState
  {
    Unknown,
    EntryOpen,
    EntryClosed,
    InProgress,
    Finished,
    Over
  }

  public enum StreamApi
  {
    Unknown,
    Twitch,
    Hitbox
  }
}
