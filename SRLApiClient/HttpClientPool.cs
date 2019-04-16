using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SRLApiClient
{
  internal sealed class HttpClientPool : IDisposable
  {
    private readonly SemaphoreSlim _clientPoolSemaphore;

    private readonly List<PoolClient> _clientPool = new List<PoolClient>();

    public HttpClientPool(HttpClientHandler handler, int poolSize, string basePath, TimeSpan requestTimeout)
    {
      handler.AllowAutoRedirect = false;

      for (int i = 0; i < poolSize; i++) _clientPool.Add(new PoolClient(handler, basePath, requestTimeout));
      _clientPoolSemaphore = new SemaphoreSlim(_clientPool.Count);
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
    {
      await _clientPoolSemaphore.WaitAsync().ConfigureAwait(false);

      PoolClient client = null;
      try
      {
        client = AquireClient();
        return await client.SendAsync(requestMessage).ConfigureAwait(false);
      }
      catch
      {
        throw;
      }
      finally
      {
        client?.Release();
        _clientPoolSemaphore.Release();
      }
    }

    public async Task<Stream> GetStreamAsync(string url)
    {
      await _clientPoolSemaphore.WaitAsync().ConfigureAwait(false);

      PoolClient client = null;
      try
      {
        client = AquireClient();
        return await client.GetStreamAsync(url).ConfigureAwait(false);
      }
      catch
      {
        throw;
      }
      finally
      {
        client?.Release();
        _clientPoolSemaphore.Release();
      }
    }

    private PoolClient AquireClient()
    {
      return _clientPool.First(c => c.TryAquire());
    }

    public void Dispose()
    {
      _clientPool.ForEach(c =>
      {
        c?.Dispose();
        c = null;
      });
    }

    private sealed class PoolClient : IDisposable
    {
      public bool IsAquired { get; private set; }

      private HttpClient _client;
      private readonly SemaphoreSlim _reqSemaphore = new SemaphoreSlim(1, 1);
      private readonly SemaphoreSlim _semaSemaphore = new SemaphoreSlim(1, 1);

      public PoolClient(HttpClientHandler handler, string basePath, TimeSpan requestTimeout)
      {
        _client = new HttpClient(handler)
        {
          BaseAddress = new Uri(basePath),
          Timeout = requestTimeout,
        };

        ProductInfoHeaderValue productInfo = new ProductInfoHeaderValue(Assembly.GetAssembly(GetType()).GetName().Name, Assembly.GetAssembly(GetType()).GetName().Version.ToString(2));

        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.UserAgent.Add(productInfo);
      }

      public bool TryAquire()
      {
        _semaSemaphore.Wait();

        bool oldState = IsAquired;
        IsAquired = IsAquired ? IsAquired : !IsAquired;

        _semaSemaphore.Release();

        return !oldState;
      }

      public void Release()
      {
        _semaSemaphore.Wait();
        IsAquired = false;
        _semaSemaphore.Release();
      }

      public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
      {
        await _reqSemaphore.WaitAsync().ConfigureAwait(false);
        try
        {
          return await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
        }
        catch
        {
          throw;
        }
        finally
        {
          _reqSemaphore.Release();
        }
      }

      public async Task<Stream> GetStreamAsync(string url)
      {
        await _reqSemaphore.WaitAsync().ConfigureAwait(false);

        try
        {
          return await _client.GetStreamAsync(url).ConfigureAwait(false);
        }
        catch
        {
          throw;
        }
        finally
        {
          _reqSemaphore.Release();
        }
      }

      public void Dispose()
      {
        _client?.Dispose();
        _client = null;
      }
    }
  }
}
