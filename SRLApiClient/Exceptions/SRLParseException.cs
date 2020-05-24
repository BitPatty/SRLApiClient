using System;
using System.Runtime.Serialization;

#pragma warning disable CS1591

namespace SRLApiClient.Exceptions
{
  public class SRLParseException : Exception
  {
    public SRLParseException()
    {
    }

    public SRLParseException(string message) : base(message)
    {
    }

    public SRLParseException(string message, Exception inner) : base(message, inner)
    {
    }

    protected SRLParseException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
