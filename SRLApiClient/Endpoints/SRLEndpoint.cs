﻿namespace SRLApiClient.Endpoints
{
  /// <summary>
  /// Base class for SRL endpoint clients
  /// </summary>
  public class SRLEndpoint
  {
    /// <summary>
    /// The endpoint that is used to peform requests on
    /// </summary>
    public string BasePath { get; protected set; }

    /// <summary>
    /// Base client to perform requests
    /// </summary>
    protected SRLClient SrlClient { get; private set; }

    /// <summary>
    /// Creates a new SRL endpoint clinet
    /// </summary>
    /// <param name="path"><see cref="BasePath"/> used for requests</param>
    /// <param name="client"><see cref="SrlClient"/> used for request</param>
    public SRLEndpoint(string path, SRLClient client)
    {
      SrlClient = client;
      BasePath = path;
    }
  }
}
