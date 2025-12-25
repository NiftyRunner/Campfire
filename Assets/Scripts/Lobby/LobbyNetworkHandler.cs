using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;


public class LobbyNetworkHandler : MonoBehaviour
{
    public static LobbyNetworkHandler Instance { get; private set; }

    public const string KEY_PLAYER_NAME = "PlayerName";

    //Event Actions
    public static event Action<Lobby> OnJoinedLobby;

    private string playerName = "";
    private Lobby joinedLobby;

    private void Awake()
    {
        Instance = this;
    }

    public async void Authenticate(string _playerName) //Make a single use button for authentication
    {
        playerName = _playerName;
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);


        await UnityServices.InitializeAsync(initializationOptions);
       
        AuthenticationService.Instance.SignedIn += () =>
        {
            
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateLobby(string lobbyName, int maxPlayers,bool isPrivate)
    {
        Player player = GetPlayer();

        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Player = player,
            IsPrivate = isPrivate
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

        joinedLobby = lobby;

        OnJoinedLobby?.Invoke(lobby);
        Debug.Log("Lobby created with id: " + lobby.Id + "Lobby code: "+ lobby.LobbyCode);
    }

    private Player GetPlayer()
    {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject>
              {
                  { KEY_PLAYER_NAME,  new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }   
              });
    }

}
