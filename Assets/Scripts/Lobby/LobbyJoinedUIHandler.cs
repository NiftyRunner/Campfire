using TMPro;
using UnityEngine;
using UnityEngine.UI; 
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;

public class LobbyJoinedUIHandler : MonoBehaviour
{
    public static LobbyJoinedUIHandler Instance { get; private set; }

    //Temp variables
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject menuCamera;

    [SerializeField] private Button leaveLobbyButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private TextMeshProUGUI lobbyName;
    [SerializeField] private TextMeshProUGUI playerCount;
    [SerializeField] private TextMeshProUGUI lobbyCode;
    [SerializeField] private Transform container;

    [SerializeField] private Transform playerInstanceTemplate;

    private void Awake()
    {
        Instance = this;

        playerInstanceTemplate.gameObject.SetActive(false);

        leaveLobbyButton.onClick.AddListener(() =>
        {
            LobbyNetworkHandler.Instance.LeaveLobby();

        });

        startGameButton.onClick.AddListener(() => {
            LobbyNetworkHandler.Instance.StartGame();
        });

    }

    private void OnEnable()
    {
        LobbyNetworkHandler.OnJoinedLobby += LobbyNetworkHandler_UpdateLobby;
        LobbyNetworkHandler.OnJoinedLobbyUpdate += LobbyNetworkHandler_UpdateLobby;
        LobbyNetworkHandler.OnKickedFromLobby += LobbyNetworkHandler_OnKickedFromLobby;
        LobbyNetworkHandler.OnLeftLobby += LobbyNetworkHandler_OnLeftLobby;
        LobbyNetworkHandler.OnGameStarted += LobbyNetworkHandler_OnGameStarted;
    }

    private void OnDisable()
    {
        LobbyNetworkHandler.OnJoinedLobby -= LobbyNetworkHandler_UpdateLobby;
        LobbyNetworkHandler.OnJoinedLobbyUpdate -= LobbyNetworkHandler_UpdateLobby;
        LobbyNetworkHandler.OnKickedFromLobby -= LobbyNetworkHandler_OnKickedFromLobby;
        LobbyNetworkHandler.OnLeftLobby -= LobbyNetworkHandler_OnLeftLobby;
        LobbyNetworkHandler.OnGameStarted -= LobbyNetworkHandler_OnGameStarted;
    }

    private void Start()
    {
        
        ClearLobby();
    }

    private void LobbyNetworkHandler_UpdateLobby(Lobby obj)
    {
        UpdateLobby();
    }

    private void UpdateLobby()
    {
        UpdateLobby(LobbyNetworkHandler.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby)
    {
        ClearLobby();
        playerInstanceTemplate.gameObject.SetActive(true);
        foreach (Player player in lobby.Players) {
            Transform playerSingleTransform = Instantiate(playerInstanceTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);

            LobbyPlayerUI playerUI = playerSingleTransform.GetComponentInChildren<LobbyPlayerUI>();

            bool isHostOnly = LobbyNetworkHandler.Instance.IsLobbyHost() &&
                    player.Id != AuthenticationService.Instance.PlayerId;

            startGameButton.gameObject.SetActive(LobbyNetworkHandler.Instance.IsLobbyHost());

            playerUI.SetKickPlayerButtonVisible(
                    isHostOnly
                );

            playerUI.UpdatePlayer(player);
        }

        lobbyName.text = lobby.Name;
        playerCount.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        lobbyCode.text = lobby.LobbyCode;

        LobbyPanelRelationHandler.Instance.SetLobbyJoinedPanel(true);

    }

    private void LobbyNetworkHandler_OnKickedFromLobby(Lobby obj)
    {
        LobbyNetworkHandler_OnLeftLobby();
    }
    private void LobbyNetworkHandler_OnLeftLobby()
    {
        ClearLobby();

        Hide();
    }

    private void LobbyNetworkHandler_OnGameStarted()
    {
        mainCanvas.SetActive(false);
        menuCamera.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ClearLobby()
    {
        foreach (Transform child in container)
        {
            if (child == playerInstanceTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void Hide()
    {
        LobbyPanelRelationHandler.Instance.SetLobbyJoinedPanel(false); //Setting lobby joined panel off
        LobbyPanelRelationHandler.Instance.SetLobbyPanel(true); //Setting lobby list panel on
    }

}
