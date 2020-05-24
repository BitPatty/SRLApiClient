using System;
using System.Runtime.Serialization;

#pragma warning disable RCS1163

namespace SRLApiClient.Endpoints
{
  /// <summary>
  /// Base class for objects returned by the SRL API
  /// </summary>
  [DataContract]
  public class SRLData
  {
    /// <summary>
    /// The timestamp of when the endpoint was deserialized
    /// </summary>
    public DateTime Deserialized { get; private set; }

    [OnDeserialized()]
    private void SetTimeStamp(StreamingContext context)
      => Deserialized = DateTime.Now;
  }
}
