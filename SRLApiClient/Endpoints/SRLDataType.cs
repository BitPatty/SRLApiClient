using System;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints
{
  [DataContract]
  public class SRLDataType
  {
    /// <summary>
    /// Timestamp of when the endpoint was deserialized
    /// </summary>
    public DateTime Deserialized { get; private set; }

    [OnDeserialized()]
    private void SetTimeStamp(StreamingContext context)
    {
      Deserialized = DateTime.Now;
    }
  }
}
