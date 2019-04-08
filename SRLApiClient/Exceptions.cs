using System;
using System.Runtime.Serialization;

#pragma warning disable CS1591

namespace SRLApiClient
{

  public class SRLTimeoutException : Exception
  {
    public SRLTimeoutException() { }
    public SRLTimeoutException(string message) : base(message) { }
    public SRLTimeoutException(string message, Exception inner) : base(message, inner) { }
    protected SRLTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }

  public class SRLParseException : Exception
  {
    public SRLParseException() { }
    public SRLParseException(string message) : base(message) { }
    public SRLParseException(string message, Exception inner) : base(message, inner) { }
    protected SRLParseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
