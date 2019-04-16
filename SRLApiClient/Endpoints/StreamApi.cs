namespace SRLApiClient.Endpoints
{
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
