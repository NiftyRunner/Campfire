using TMPro;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;

public class LobbyJoinByCodeHandler : MonoBehaviour
{
    [SerializeField] private Button backToLobbyListButton;
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private TMP_InputField lobbyCode;


    private void OnEnable()
    {
        LobbyNetworkHandler.OnJoinedLobby += LobbyNetworkHandler_OnJoinedLobby;
        LobbyNetworkHandler.OnLobbyJoinFail += LobbyNetworkHandler_OnLobbyJoinFail;
    }

    private void OnDisable()
    {
        LobbyNetworkHandler.OnJoinedLobby -= LobbyNetworkHandler_OnJoinedLobby;
        LobbyNetworkHandler.OnLobbyJoinFail -= LobbyNetworkHandler_OnLobbyJoinFail;
    }

    private void Start()
    {

        backToLobbyListButton.onClick.AddListener(() => {
            LobbyPanelRelationHandler.Instance.SetLobbyPanel(true);
            LobbyPanelRelationHandler.Instance.SetFindLobbyPanel(false);
        });

        joinLobbyButton.onClick.AddListener(() => {
            LobbyNetworkHandler.Instance.JoinLobbyWithCode(lobbyCode.text);
            VivoxHandler.Instance.JoinVoiceChannel(lobbyCode.text);
        });
    }

    private void LobbyNetworkHandler_OnLobbyJoinFail()
    {
        lobbyCode.text = "Invalid!";
        Invoke(nameof(ClearInputField), 1f);
    }

    private void LobbyNetworkHandler_OnJoinedLobby(Unity.Services.Lobbies.Models.Lobby obj)
    {
        gameObject.SetActive(false);
        LobbyPanelRelationHandler.Instance.SetLobbyJoinedPanel(true);
    }

    private void ClearInputField()
    {
        lobbyCode.text = string.Empty;
    }
}
