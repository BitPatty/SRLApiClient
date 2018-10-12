using System;
using System.Runtime.Serialization;

#pragma warning disable CS1591

namespace SRLApiClient
{
  public class SRLApiException : Exception
  {
    public SRLApiException() { }
    public SRLApiException(string message) : base(message) { }
    public SRLApiException(string message, Exception inner) : base(message, inner) { }
    protected SRLApiException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
