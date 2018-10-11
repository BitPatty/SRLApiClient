/*
 * SRLApiClient - A .NET client library for the SpeedRunsLive API
 * Copyright (c) 2018 Matt Collet
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
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SRLApiClient.Endpoints;

namespace SRLApiClient
{
  public sealed partial class SRLClient : IDisposable
  {
    public string Host { get; private set; }
    private string _baseDomain => "." + Host;
    private string _apiUrl => "http://api." + Host;
    private string _authUrl => "http://login." + Host + ":9000/login";
    private string _baseUrl => "http://" + Host;

    /// <summary>
    /// User account associated with the client if authenticated.
    /// </summary>
    public SRLUser User { get; private set; }
    private string _userName { get; set; }
    private string _userPassword { get; set; }

    /// <summary>
    /// True if client uses an authenticated account
    /// </summary>
    public bool IsAuthenticated { get; private set; }

    /// <summary>
    /// /games endpoint
    /// </summary>
    public Endpoints.Games.GamesClient Games { get; private set; }

    /// <summary>
    /// /players endpoint
    /// </summary>
    public Endpoints.Players.PlayersClient Players { get; private set; }

    /// <summary>
    /// /leaderboards endpoint
    /// </summary>
    public Endpoints.Leaderboards.LeaderboardsClient Leaderboards { get; private set; }

    /// <summary>
    /// /races endpoint
    /// </summary>
    public Endpoints.Races.RacesClient Races { get; private set; }

    /// <summary>
    /// /pastraces endpoint
    /// </summary>
    public Endpoints.PastRaces.PastRacesClient PastRaces { get; private set; }

    /// <summary>
    /// /country endpoint
    /// </summary>
    public Endpoints.Countries.CountriesClient Countries { get; private set; }

    /// <summary>
    /// /stat endpoint
    /// </summary>
    public Endpoints.Stats.StatsClient Stats { get; private set; }

    private TimeSpan _requestTimeout = TimeSpan.FromMilliseconds(10000);

    /// <summary>
    /// Timeout for Http Requests
    /// </summary>
    public TimeSpan RequestTimeout
    {
      get => _requestTimeout;
      set { _requestTimeout = (value < TimeSpan.FromMilliseconds(1000)) ? value : TimeSpan.FromMilliseconds(1000); }
    }

    private CookieContainer _cookieJar = new CookieContainer();
    private readonly HttpClientHandler _clientHandler = new HttpClientHandler();
    private readonly HttpClient _client;
    private readonly SemaphoreSlim _slim = new SemaphoreSlim(1, 1);

    /// <summary>
    /// Create a new SRL Client
    /// </summary>
    /// <param name="host">Custom host</param>
    public SRLClient(string host = "speedrunslive.com")
    {
      if (String.IsNullOrWhiteSpace(host)) throw new ArgumentException(nameof(host), "Parameter cannot be empty");

      Host = host;
      _clientHandler.CookieContainer = _cookieJar;
      _clientHandler.AllowAutoRedirect = false;

      _client = new HttpClient(_clientHandler)
      {
        BaseAddress = new Uri(_baseUrl),
        Timeout = _requestTimeout,
      };

      _client.DefaultRequestHeaders.Accept.Clear();
      _client.DefaultRequestHeaders.Add("User-Agent", new List<string>() {
        Assembly.GetCallingAssembly().GetName().Name + " " + Assembly.GetCallingAssembly().GetName().Version,
        Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version
      });

      Games = new Endpoints.Games.GamesClient(this);
      Players = new Endpoints.Players.PlayersClient(this);
      Leaderboards = new Endpoints.Leaderboards.LeaderboardsClient(this);
      Races = new Endpoints.Races.RacesClient(this);
      PastRaces = new Endpoints.PastRaces.PastRacesClient(this);
      Countries = new Endpoints.Countries.CountriesClient(this);
      Stats = new Endpoints.Stats.StatsClient(this);
    }

    /// <summary>
    /// Perform a GET request on an endpoint
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="endpoint">Endpoint to perform the request on</param>
    /// <param name="res">Deserialized response</param>
    /// <returns>Returns true if the request and deserialization are successful</returns>
    public bool Get<T>(string endpoint, out T res) where T : SRLDataType
    {
      string ep = endpoint.StartsWith('/') ? endpoint : '/' + endpoint;
      res = default(T);
      return GetStream(_apiUrl + ep, out Stream s) && s != null && DeSerialize(s, out res);
    }

    /// <summary>
    /// Perform a PUT request on an endpoint
    /// </summary>
    /// <param name="endpoint">Endpoint to perform the request on</param>
    /// <param name="data">PUT data</param>
    /// <returns>Returns true if the PUT request was successful</returns>
    public bool Put(string endpoint, Dictionary<string, string> data) => SubmitJson(endpoint, data, HttpMethod.Put, out _);

    /// <summary>
    /// Perform a PUT request on an endpoint
    /// </summary>
    /// <param name="endpoint">Endpoint to perform the request on</param>
    /// <param name="data">PUT data</param>
    /// <param name="response">The HTTP response received from the endpoint</param>
    /// <returns>Returns true if the PUT request was successful</returns>
    public bool Put(string endpoint, Dictionary<string, string> data, out HttpResponseMessage response) => SubmitJson(endpoint, data, HttpMethod.Put, out response);

    /// <summary>
    /// Perform a POST request on an endpoint
    /// </summary>
    /// <param name="endpoint">Endpoint to perform the request on</param>
    /// <param name="data">PUT data</param>
    /// <returns>Returns true if the PUT request was successful</returns>
    public bool Post(string endpoint, Dictionary<string, string> data) => SubmitJson(endpoint, data, HttpMethod.Post, out _);

    /// <summary>
    /// Perform a POST request on an endpoint
    /// </summary>
    /// <param name="endpoint">Endpoint to perform the request on</param>
    /// <param name="data">PUT data</param>
    /// <param name="response">The HTTP response received from the endpoint</param>
    /// <returns>Returns true if the PUT request was successful</returns>
    public bool Post(string endpoint, Dictionary<string, string> data, out HttpResponseMessage response) => SubmitJson(endpoint, data, HttpMethod.Post, out response);

    private bool SubmitJson(string endpoint, Dictionary<string, string> data, HttpMethod method, out HttpResponseMessage response)
    {
      string ep = endpoint.StartsWith('/') ? endpoint : '/' + endpoint;
      string s = Serialize(data);
      _slim.Wait();

      HttpRequestMessage req = new HttpRequestMessage(method, _apiUrl + ep);
      req.Content = new StringContent(s);
      req.Headers.Accept.Clear();
      req.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

      try
      {
        HttpResponseMessage r = _client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead).Result;
        response = r;
        return r.IsSuccessStatusCode;
      }
      catch (HttpRequestException ex)
      {
        Console.WriteLine(ex.Message);
        response = null;
        return false;
      }
      finally
      {
        _slim.Release();
      }
    }

    /// <summary>
    /// Deserializes a JSON stream
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s">The stream</param>
    /// <param name="res">The deserialized object</param>
    /// <returns>Returns true if the deserialization was successful</returns>
    public bool DeSerialize<T>(Stream s, out T res) where T : SRLDataType
    {
      DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
      try
      {
        res = (T)serializer.ReadObject(s);
        return res != null;
      }
      catch (SerializationException ex)
      {
        Console.WriteLine(ex.Message);
        res = default(T);
      }

      return false;
    }

    private string Serialize(Dictionary<string, string> postData)
    {
      string s = "{";
      foreach (KeyValuePair<string, string> kvp in postData) s += String.Format("{0}:{1},", HttpUtility.JavaScriptStringEncode(kvp.Key, true), HttpUtility.JavaScriptStringEncode(kvp.Value, true));
      return s.TrimEnd(',') + '}';
    }

    private bool GetStream(string url, out Stream data)
    {
      _slim.Wait();
      try
      {
        data = _client.GetStreamAsync(url).Result;
        return true;
      }
      catch (TaskCanceledException ex)
      {
        Console.WriteLine(ex.Message);
        data = null;
      }
      finally
      {
        _slim.Release();
      }

      return false;
    }

    /// <summary>
    /// Delete user information associated with the client
    /// </summary>
    public void Logout()
    {
      _slim.Wait();
      _cookieJar = new CookieContainer();
      _clientHandler.CookieContainer = _cookieJar;
      _userName = null;
      _userPassword = null;
      User = null;
      IsAuthenticated = false;
      _slim.Release();
    }

    /// <summary>
    /// Reauthenticate with the stored credentials
    /// </summary>
    /// <returns>Returns true if the authentication was successful</returns>
    public bool ReAuthenticate() => Authenticate(_userName, _userPassword, true);

    /// <summary>
    /// Asssociate the client with a user account
    /// </summary>
    /// <param name="username">SRL Username</param>
    /// <param name="password">SRL Password</param>
    /// <param name="storeCredentials">If True the password is stored in the client</param>
    /// <returns>Returns true if the authentication was successful</returns>
    public bool Authenticate(string username, string password, bool storeCredentials = false)
    {
      if (String.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username), "Parameter can't be empty");
      if (String.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password), "Parameter can't be empty");

      if (IsAuthenticated) Logout();

      _slim.Wait();
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

      HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, _authUrl);
      req.Content = new FormUrlEncodedContent(x_form);
      req.Headers.Accept.Clear();
      req.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

      try
      {
        HttpResponseMessage resp = _client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead).Result;
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
                for (int i = 1; i < c_key.Length; i++) cookie.Add(c_key[i].Trim().Split('=', 2)[0].ToLower(), c_key[i].Trim().Split('=', 2)[1]);

                string c_name = c_key[0].Split('=')[0];
                string c_val = c_key[0].Split('=', 2)[1];
                string c_path = cookie.ContainsKey("path") ? cookie["path"] : "";
                string c_domain = cookie.ContainsKey("domain") ? cookie["domain"] : _baseDomain;
                DateTime c_expires = cookie.ContainsKey("expires") && DateTime.TryParse(cookie["expires"], out DateTime dt) ?
                  dt : cookie.ContainsKey("max-age") && Int32.TryParse(cookie["max-age"], out int res) ?
                  DateTime.Now + TimeSpan.FromMilliseconds(res) : default(DateTime);

                _cookieJar.Add(new Cookie(c_name, c_val, c_path, _baseDomain) { Expires = c_expires });
              }
            }

            req = new HttpRequestMessage(HttpMethod.Get, _apiUrl + "/token/login");
            req.Headers.Accept.Clear();
            req.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            resp = _client.SendAsync(req).Result;

            if (resp.IsSuccessStatusCode)
            {
              User = new SRLUser(this);
            }
          }
        }
      }
      catch (HttpRequestException ex)
      {
        Console.WriteLine(ex.Message);
      }
      finally
      {
        _slim.Release();
      }

      return User?.Verify() ?? false;
    }

    public void Dispose()
    {
      _client.Dispose();
      _clientHandler.Dispose();
      _slim.Dispose();
    }
  }
}

