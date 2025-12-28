using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItemSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playersText;
    //[SerializeField] private TextMeshProUGUI gameModeText;


    private Lobby lobby;

    private GameObject lobbyJoinedPanel;

    private void Start()
    {
        lobbyJoinedPanel = LobbyPanelRelationHandler.Instance.GetLobbyJoinedPanel();

        GetComponent<Button>().onClick.AddListener(() =>
        {
             LobbyNetworkHandler.Instance.JoinLobby(lobby);
            lobbyJoinedPanel.SetActive(true);
        });
    }

    public void UpdateLobby(Lobby lobby)
    {
        this.lobby = lobby;

        lobbyNameText.text = lobby.Name;
        playersText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        //gameModeText.text = lobby.Data[LobbyManager.KEY_GAME_MODE].Value;
    }
}
