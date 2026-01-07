using StarterAssets;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerRegistry
{
    public static readonly Dictionary<string, LobbyPlayerUI> lobbyPlayers = new Dictionary<string, LobbyPlayerUI>();

    public static readonly Dictionary<string, FirstPersonController> gamePlayers = new Dictionary<string, FirstPersonController>();
}
