﻿using System;
using System.Runtime.Serialization;

namespace SRLApiClient.Endpoints.Stats
{
  public class StatsClient : SRLEndpoint
  {
    public StatsClient(SRLClient baseClient) : base("/stat", baseClient) { }

    [DataContract, KnownType(typeof(SRLDataType))]
    private sealed class PlayerEndpoint : SRLDataType
    {
      [DataMember(Name = "stats", IsRequired = true)]
      public PlayerStats Stats { get; private set; }
    }

    /// <summary>
    /// Gets the statistics for a player
    /// </summary>
    /// <param name="playerName">The name of the player</param>
    /// <param name="gameAbbrevation">The game abbrevation</param>
    /// <returns>Returns the player stats or null</returns>
    public PlayerStats GetPlayerStats(string playerName, string gameAbbrevation = null)
    {
      if (string.IsNullOrWhiteSpace(playerName)) throw new ArgumentException(nameof(playerName), "Parameter can't be empty");
      if (gameAbbrevation == null) return SrlClient.Get(BasePath + "?player=" + playerName, out PlayerEndpoint ps) ? ps.Stats : null;
      else return SrlClient.Get(BasePath + "?player=" + playerName + "&game=" + gameAbbrevation, out PlayerEndpoint ps) ? ps.Stats : null;
    }

    [DataContract, KnownType(typeof(SRLDataType))]
    private sealed class GameEndpoint : SRLDataType
    {
      [DataMember(Name = "stats", IsRequired = true)]
      public GameStats Stats { get; private set; }
    }

    /// <summary>
    /// Gets the statistics for a single game
    /// </summary>
    /// <param name="gameAbbrevation">The game abbrevation</param>
    /// <returns>Returns the game stats or null</returns>
    public GameStats GetGameStats(string gameAbbrevation)
    {
      if (string.IsNullOrWhiteSpace(gameAbbrevation)) throw new ArgumentException(nameof(gameAbbrevation), "Parameter can't be empty");
      return SrlClient.Get(BasePath + "?game=" + gameAbbrevation, out GameEndpoint ge) ? ge.Stats : null;
    }
  }
}
