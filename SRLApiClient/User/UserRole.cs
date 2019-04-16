namespace SRLApiClient.User
{
  /// <summary>
  /// The users permission level
  /// </summary>
  public enum UserRole
  {
    /// <summary>
    /// Fallback value
    /// </summary>
    Unknown,

    /// <summary>
    /// Anonymous permissions
    /// </summary>
    Anon,

    /// <summary>
    /// User permissions
    /// </summary>
    User,

    /// <summary>
    /// Voice permissions
    /// </summary>
    Voice,

    /// <summary>
    /// Half Operator permissions
    /// </summary>
    Halfop,

    /// <summary>
    /// Operator permissions
    /// </summary>
    Op,

    /// <summary>
    /// Admin permissions
    /// </summary>
    Admin
  }
}
