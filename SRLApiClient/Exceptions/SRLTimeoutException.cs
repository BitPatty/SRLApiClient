using System;
using System.Runtime.Serialization;

#pragma warning disable CS1591

namespace SRLApiClient.Exceptions
{
  public class SRLTimeoutException : Exception
  {
    public SRLTimeoutException() { }
    public SRLTimeoutException(string message) : base(message) { }
    public SRLTimeoutException(string message, Exception inner) : base(message, inner) { }
    protected SRLTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
