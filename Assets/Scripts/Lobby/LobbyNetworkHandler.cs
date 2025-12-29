using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;


public class LobbyNetworkHandler : MonoBehaviour
{
    public static LobbyNetworkHandler Instance { get; private set; }

    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_START_GAME = "Start";

    //Event Actions
    public static event Action<Lobby> OnJoinedLobby;
    public static event Action<Lobby> OnJoinedLobbyUpdate;
    public static event Action<Lobby> OnKickedFromLobby;
    public static event Action OnLeftLobby;
    public static event Action OnLobbyJoinFail;

    public static event Action OnGameStarted;


    public static event Action<List<Lobby>> OnLobbyListUpdate;

    private string playerName = "";
    private float heartbeatTimer;
    private float lobbyPollTimer;
    private float refreshLobbyListTimer = 5f;
    private Lobby joinedLobby;

    private List<Lobby> lobbyList;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPolling();
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

    public async void CreateLobby(string lobbyName, int maxPlayers, bool isPrivate)
    {
        Player player = GetPlayer();

        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Player = player,
            IsPrivate = isPrivate,
            Data = new Dictionary<string, DataObject>
            {
                {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") }
            }
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

        joinedLobby = lobby;

        OnJoinedLobby?.Invoke(lobby);
        Debug.Log("Lobby created with id: " + lobby.Id + "Lobby code: " + lobby.LobbyCode);
    }

    private Player GetPlayer()
    {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject>
              {
                  { KEY_PLAYER_NAME,  new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
              });
    }

    public Lobby GetJoinedLobby()
    {
        return joinedLobby;
    }

    //Keeping lobby alive by sending ping every 15 seconds
    private async void HandleLobbyHeartbeat()
    {
        if (IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer <= 0f)
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;
                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }


    //Updating lobby every 1.1 second
    private async void HandleLobbyPolling()
    {
        if (joinedLobby != null)
        {
            lobbyPollTimer -= Time.deltaTime;
            if (lobbyPollTimer <= 0f)
            {
                float maxLobbyPollTimer = 1.1f;
                lobbyPollTimer = maxLobbyPollTimer;

                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

                OnJoinedLobbyUpdate?.Invoke(joinedLobby);

                if (!IsPlayerInLobby())
                {
                    Debug.Log("Kicked from lobby");
                    OnKickedFromLobby?.Invoke(joinedLobby);
                    joinedLobby = null;
                }

                if (joinedLobby.Data[KEY_START_GAME].Value != "0")
                {
                    if (!IsLobbyHost())
                    {
                        RelayHandler.Instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
                    }

                    joinedLobby = null;
                    OnGameStarted?.Invoke();
                }
            }
        }
    }


    public async void RefreshLobbyList()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();

            options.Count = 25;

            options.Filters = new List<QueryFilter> {new QueryFilter(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0"
                ) };

            QueryResponse lobbyListQueryResponse = await LobbyService.Instance.QueryLobbiesAsync(options);

            lobbyList = lobbyListQueryResponse.Results;

            OnLobbyListUpdate?.Invoke(lobbyList);
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    public async void JoinLobby(Lobby _lobby)
    {
        Player player = GetPlayer();

        joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(_lobby.Id, new JoinLobbyByIdOptions { Player = player });

        OnJoinedLobby?.Invoke(_lobby);
    }

    public async void JoinLobbyWithCode(string lobbyCode)
    {
        try
        {
            Player player = GetPlayer();

            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, new JoinLobbyByCodeOptions { Player = player });

            joinedLobby = lobby;

            OnJoinedLobby?.Invoke(lobby);
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
            OnLobbyJoinFail?.Invoke();
        }
    }

    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                joinedLobby = null;
                OnLeftLobby?.Invoke();
            }
            catch (LobbyServiceException ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException ex)
            {
                Debug.Log(ex);
            }
        }
    }

    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private bool IsPlayerInLobby()
    {
        if (joinedLobby != null && joinedLobby.Players != null)
        {
            foreach (Player player in joinedLobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    // This player is in this lobby
                    return true;
                }
            }
        }
        return false;
    }

    public async void StartGame()
    {
        if (IsLobbyHost())
        {
            try
            {
                string relayCode = await RelayHandler.Instance.CreateRelay();

                Lobby lobby = await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions {
                    Data = new Dictionary<string, DataObject>
                    {
                        {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });

                joinedLobby = lobby;

            }
            catch (LobbyServiceException ex) { 
                Debug.Log(ex);
            }
        }
    }
}