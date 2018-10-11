using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints
{
  public class SRLEndpoint
  {
    public string BasePath { get; protected set; }
    protected SRLClient SrlClient { get; private set; }

    public SRLEndpoint(string path, SRLClient client)
    {
      SrlClient = client;
      this.BasePath = path;
    }

    [DataContract]
    protected sealed class GameRules : SRLDataType
    {
      [DataMember(Name = "rules", IsRequired = true)]
      public string Rules { get; private set; }
    }
  }
}
