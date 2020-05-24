using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace SRLApiClient
{
  internal sealed class HttpClient : IDisposable
  {
    private readonly BaseClient _baseClient;

    public HttpClient(HttpClientHandler handler, string basePath, TimeSpan requestTimeout)
    {
      handler.AllowAutoRedirect = false;
      _baseClient = new BaseClient(handler, basePath, requestTimeout);
    }

    public void SetRequestTimeout(TimeSpan timespan)
     => _baseClient.SetTimeout(timespan);

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        => await _baseClient.SendAsync(requestMessage).ConfigureAwait(false);

    public async Task<Stream> GetStreamAsync(string url)
   => await _baseClient.GetStreamAsync(url).ConfigureAwait(false);

    public void Dispose()
    {
      try
      {
        _baseClient.Dispose();
      }
      catch (ObjectDisposedException) { }
    }

    private sealed class BaseClient : IDisposable
    {
      private System.Net.Http.HttpClient _client;

      public BaseClient(HttpClientHandler handler, string basePath, TimeSpan requestTimeout)
      {
        _client = new System.Net.Http.HttpClient(handler)
        {
          BaseAddress = new Uri(basePath),
          Timeout = requestTimeout,
        };

        ProductInfoHeaderValue productInfo = new ProductInfoHeaderValue(
            Assembly
                .GetAssembly(GetType())
                .GetName()
                .Name
            , Assembly
                .GetAssembly(GetType())
                .GetName()
                .Version
                .ToString(2)
        );

        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.UserAgent.Add(productInfo);
      }

      public void SetTimeout(TimeSpan timeout)
       => _client.Timeout = timeout;

      public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        => await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

      public async Task<Stream> GetStreamAsync(string url)
        => await _client.GetStreamAsync(url).ConfigureAwait(false);

      public void Dispose()
      {
        try
        {
          _client?.Dispose();
          _client = null;
        }
        catch (ObjectDisposedException) { }
      }
    }
  }
}
