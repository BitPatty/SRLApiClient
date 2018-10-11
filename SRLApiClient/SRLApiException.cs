using System;

namespace SRLApiClient
{
  public class SRLApiException : Exception
  {
    public SRLApiException() { }
    public SRLApiException(string message) : base(message) { }
    public SRLApiException(string paramName, string message) : base(String.Format("{0}: {1}", paramName, message)) { }
    public SRLApiException(string message, Exception inner) : base(message, inner) { }
  }
}
