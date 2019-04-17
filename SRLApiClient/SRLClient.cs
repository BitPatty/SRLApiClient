/*
 * SRLApiClient - A .NET client library for the SpeedRunsLive API
 * Copyright (c) 2018 - 2019 Matteias Collet
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SRLApiClient.Endpoints;
using SRLApiClient.Exceptions;
using SRLApiClient.User;

namespace SRLApiClient
{
  /// <summary>
  /// A client for interacting with the SpeedRunsLive API
  /// </summary>
  public sealed class SRLClient : IDisposable
  {
    /// <summary>
    /// The host used for requests
    /// </summary>
    public string Host { get; private set; }
    private string _baseDomain => "." + Host;
    private string _apiUrl => "http://api." + Host;
    private string _authUrl => "http://login." + Host + ":9000/login";
    private string _baseUrl => "http://" + Host;

    private readonly HttpClientPool _clientPool;
    private CookieContainer _cookieJar = new CookieContainer();
    private readonly HttpClientHandler _clientHandler = new HttpClientHandler();

    /// <summary>
    /// The user account associated with the client (if authenticated)
    /// </summary>
    public SRLUser User { get; private set; }
    private string _userName { get; set; }
    private string _userPassword { get; set; }

    /// <summary>
    /// True if client uses an authenticated account
    /// </summary>
    public bool IsAuthenticated { get => User?.Verify() ?? false; }

    /// <summary>
    /// Client to perform request on the /games endpoint
    /// </summary>
    public Endpoints.Games.GamesClient Games { get; private set; }

    /// <summary>
    /// Client to perform request on the /players endpoint
    /// </summary>
    public Endpoints.Players.PlayersClient Players { get; private set; }

    /// <summary>
    /// Client to perform request on the /leaderboards endpoint
    /// </summary>
    public Endpoints.Leaderboards.LeaderboardsClient Leaderboards { get; private set; }

    /// <summary>
    /// Client to perform request on the /races endpoint
    /// </summary>
    public Endpoints.Races.RacesClient Races { get; private set; }

    /// <summary>
    /// Client to perform request on the /pastraces endpoint
    /// </summary>
    public Endpoints.PastRaces.PastRacesClient PastRaces { get; private set; }

    /// <summary>
    /// Client to perform request on the /country endpoint
    /// </summary>
    public Endpoints.Countries.CountriesClient Countries { get; private set; }

    /// <summary>
    /// Client to perform request on the /stat endpoint
    /// </summary>
    public Endpoints.Stats.StatsClient Stats { get; private set; }

    private TimeSpan _requestTimeout = TimeSpan.FromMilliseconds(10000);

    /// <summary>
    /// The timeout for Http Requests
    /// </summary>
    public TimeSpan RequestTimeout
    {
      get => _requestTimeout;
      set
      {
        _requestTimeout = (value < TimeSpan.FromMilliseconds(1000)) ? value : TimeSpan.FromMilliseconds(1000);
        _clientPool?.SetRequestTimeout(_requestTimeout);
      }
    }

    /// <summary>
    /// Initializes a new SRL Client
    /// </summary>
    /// <param name="poolSize">The HTTP client pool size</param>
    /// <param name="host">The custom host</param>
    public SRLClient(int poolSize = 1, string host = "speedrunslive.com")
    {
      if (String.IsNullOrWhiteSpace(host)) throw new ArgumentException(nameof(host), "Parameter cannot be empty");

      Host = host;
      _clientHandler.CookieContainer = _cookieJar;
      _clientPool = new HttpClientPool(_clientHandler, poolSize, _baseUrl, RequestTimeout);

      Games = new Endpoints.Games.GamesClient(this);
      Players = new Endpoints.Players.PlayersClient(this);
      Leaderboards = new Endpoints.Leaderboards.LeaderboardsClient(this);
      Races = new Endpoints.Races.RacesClient(this);
      PastRaces = new Endpoints.PastRaces.PastRacesClient(this);
      Countries = new Endpoints.Countries.CountriesClient(this);
      Stats = new Endpoints.Stats.StatsClient(this);
    }

    /// <summary>
    /// Performs a GET request on an endpoint
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response into</typeparam>
    /// <param name="endpoint">The endpoint to perform the request on</param>
    /// <returns>Returns the parsed response</returns>
    /// <exception cref="SRLParseException" />
    /// <exception cref="SRLTimeoutException" />
    public T Get<T>(string endpoint) where T : SRLData => GetAsync<T>(endpoint).Result;

    /// <summary>
    /// Performs an asynchronous GET request on an endpoint
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response into</typeparam>
    /// <param name="endpoint">The endpoint to perform the request on</param>
    /// <returns>Returns the parsed response</returns>
    /// <exception cref="SRLParseException" />
    /// <exception cref="SRLTimeoutException" />
    public async Task<T> GetAsync<T>(string endpoint) where T : SRLData
    {
      using (Stream responseStream = await GetStreamAsync($"{_apiUrl}/{endpoint.TrimStart('/')}").ConfigureAwait(false))
      {
        try
        {
          return DeSerialize<T>(responseStream);
        }
        catch (SerializationException ex)
        {
          throw new SRLParseException("Failed to deserialize the response stream", ex);
        }
      }
    }

    /// <summary>
    /// Performs a PUT request on an endpoint
    /// </summary>
    /// <param name="endpoint">The endpoint to perform the request on</param>
    /// <param name="data">The data to PUT</param>
    /// <returns>Returns true if the endpoint responds with HTTP 200</returns>
    public bool Put(string endpoint, Dictionary<string, string> data) => PutAsync(endpoint, data).Result;

    /// <summary>
    /// Performs an asynchronous PUT request on an endpoint
    /// </summary>
    /// <param name="endpoint">The endpoint to perform the request on</param>
    /// <param name="data">The data to PUT</param>
    /// <returns>Returns true if the endpoint responds with HTTP 200</returns>
    public async Task<bool> PutAsync(string endpoint, Dictionary<string, string> data)
    {
      string ep = endpoint.StartsWith("/") ? endpoint : "/" + endpoint;
      return await SubmitPayloadAsync(ep, data, HttpMethod.Put).ConfigureAwait(false);
    }

    /// <summary>
    /// Performs a POST request on an endpoint
    /// </summary>
    /// <param name="endpoint">The endpoint to perform the request on</param>
    /// <param name="data">The data to POST</param>
    /// <returns>Returns true if the endpoint responds with HTTP 200</returns>
    public bool Post(string endpoint, Dictionary<string, string> data) => PostAsync(endpoint, data).Result;

    /// <summary>
    /// Performs an asynchronous POST request on an endpoint
    /// </summary>
    /// <param name="endpoint">The endpoint to perform the request on</param>
    /// <param name="data">The data to POST</param>
    /// <returns>Returns true if the endpoint responds with HTTP 200</returns>
    public async Task<bool> PostAsync(string endpoint, Dictionary<string, string> data)
    {
      string ep = endpoint.StartsWith("/") ? endpoint : "/" + endpoint;
      return await SubmitPayloadAsync(ep, data, HttpMethod.Post).ConfigureAwait(false);
    }

    /// <summary>
    /// Submits the specified data to the specified endpoint
    /// </summary>
    /// <param name="endpoint">The endpoint to perform the request on</param>
    /// <param name="data">The payload</param>
    /// <param name="method">The method (POST/PUT)</param>
    /// <returns>Returns true if the endpoint responds with HTTP 200</returns>
    public async Task<bool> SubmitPayloadAsync(string endpoint, Dictionary<string, string> data, HttpMethod method)
    {
      string payload = JsonSerialize(data);

      using (HttpRequestMessage req = new HttpRequestMessage(method, String.Concat(_apiUrl, endpoint)) { Content = new StringContent(payload) })
      {
        req.Headers.Accept.Clear();
        req.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        using (HttpResponseMessage r = await _clientPool.SendAsync(req).ConfigureAwait(false))
        {
          return r.IsSuccessStatusCode;
        }
      }
    }

    /// <summary>
    /// Deserializes a response stream into an <see cref="SRLData"/>
    /// </summary>
    /// <typeparam name="T">The datatype to deserialized the stream into</typeparam>
    /// <param name="s">The stream to deserialize</param>
    /// <returns>Returns the resulting object</returns>
    public T DeSerialize<T>(Stream s) where T : SRLData
    {
      DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
      return serializer.ReadObject(s) as T;
    }

    /// <summary>
    /// Serializes the dictionary into a JSON body
    /// </summary>
    /// <param name="dict">The dictionary to serialize</param>
    /// <returns>Returns the JSON string</returns>
    public string JsonSerialize(Dictionary<string, string> dict)
    {
      if (dict == null) throw new ArgumentNullException(nameof(dict), "Parameter required");

      StringBuilder s = new StringBuilder("{");

      foreach (KeyValuePair<string, string> kvp in dict)
        s.AppendFormat("{0}:{1},", HttpUtility.JavaScriptStringEncode(kvp.Key, true), HttpUtility.JavaScriptStringEncode(kvp.Value, true));

      return String.Concat(s.ToString().TrimEnd(','), '}');
    }

    /// <summary>
    /// Performs a GET request on the specified URL and returns
    /// the reponse stream
    /// </summary>
    /// <param name="url">The URL to perform the request on</param>
    /// <returns>Returns the reponse stream</returns>
    /// <exception cref="SRLTimeoutException" />
    private async Task<Stream> GetStreamAsync(string url)
    {
      try
      {
        return await _clientPool.GetStreamAsync(url).ConfigureAwait(false);
      }
      catch (TaskCanceledException ex)
      {
        throw new SRLTimeoutException("Request exceeded the timeout limit", ex);
      }
    }

    /// <summary>
    /// Deletes the user information associated with the client
    /// </summary>
    public void Logout()
    {
      _cookieJar = new CookieContainer();
      _clientHandler.CookieContainer = _cookieJar;
      _userName = null;
      _userPassword = null;
      User = null;
    }

    /// <summary>
    /// Reauthenticate the client with the stored credentials
    /// </summary>
    /// <returns>Returns true if the authentication was successful</returns>
    public bool ReAuthenticate() => !String.IsNullOrWhiteSpace(_userName) && !string.IsNullOrWhiteSpace(_userPassword) && Authenticate(_userName, _userPassword, true);

    /// <summary>
    /// Asssociate the client with a user account
    /// </summary>
    /// <param name="username">The SRL username</param>
    /// <param name="password">The SRL password</param>
    /// <param name="storeCredentials">If true the password is stored in the client for <see cref="ReAuthenticate()"/></param>
    /// <returns>Returns true if the authentication was successful</returns>
    public bool Authenticate(string username, string password, bool storeCredentials = false)
    {
      if (String.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username), "Parameter can't be empty");
      if (String.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password), "Parameter can't be empty");

      if (IsAuthenticated) Logout();

      _cookieJar = new CookieContainer();
      _clientHandler.CookieContainer = _cookieJar;

      if (storeCredentials)
      {
        _userName = username;
        _userPassword = password;
      }

      List<KeyValuePair<string, string>> x_form = new List<KeyValuePair<string, string>>() {
        new KeyValuePair<string, string>("username", username),
        new KeyValuePair<string, string>("password", password)
      };

      using (HttpRequestMessage authRequest = new HttpRequestMessage(HttpMethod.Post, _authUrl) { Content = new FormUrlEncodedContent(x_form) })
      {
        authRequest.Headers.Accept.Clear();
        authRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        try
        {
          using (HttpResponseMessage resp = _clientPool.SendAsync(authRequest).Result)
          {
            if (resp.IsSuccessStatusCode || resp.StatusCode == HttpStatusCode.MovedPermanently)
            {
              if (resp.StatusCode == HttpStatusCode.MovedPermanently)
              {
                foreach (KeyValuePair<string, IEnumerable<string>> val in resp.Headers.Where(t => t.Key.Equals("set-cookie", StringComparison.OrdinalIgnoreCase)))
                {
                  foreach (string key in val.Value)
                  {
                    string[] c_key = key.Split(';');
                    Dictionary<string, string> cookie = new Dictionary<string, string>();
                    for (int i = 1; i < c_key.Length; i++) cookie.Add(c_key[i].Trim().Split(new char[] { '=' }, 2)[0].ToLower(), c_key[i].Trim().Split(new char[] { '=' }, 2)[1]);

                    string c_name = c_key[0].Split('=')[0];
                    string c_val = c_key[0].Split(new char[] { '=' }, 2)[1];
                    string c_path = cookie.ContainsKey("path") ? cookie["path"] : "";
                    string c_domain = cookie.ContainsKey("domain") ? cookie["domain"] : _baseDomain;
                    DateTime c_expires = cookie.ContainsKey("expires") && DateTime.TryParse(cookie["expires"], out DateTime dt) ?
                      dt : cookie.ContainsKey("max-age") && Int32.TryParse(cookie["max-age"], out int res) ?
                      DateTime.Now + TimeSpan.FromMilliseconds(res) : default(DateTime);

                    _cookieJar.Add(new Cookie(c_name, c_val, c_path, c_domain) { Expires = c_expires });
                  }
                }
              }

              using (HttpRequestMessage authCheckRequest = new HttpRequestMessage(HttpMethod.Get, _apiUrl + "/token/login"))
              {
                authCheckRequest.Headers.Accept.Clear();
                authCheckRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage authCheckResponse = _clientPool.SendAsync(authCheckRequest).Result)
                {
                  if (authCheckResponse.IsSuccessStatusCode)
                  {
                    User = new SRLUser(this);
                  }
                }
              }
            }
          }
        }
        catch
        {
          throw;
        }
      }

      return User?.Verify() ?? false;
    }

    /// <summary>
    /// Releases the unmanaged resources and disposes of the managed resources
    /// used by the <see cref="SRLClient"/>
    /// </summary>
    public void Dispose()
    {
      _clientHandler?.Dispose();
      _clientPool?.Dispose();
    }
  }
}
