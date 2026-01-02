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

    private void Start()
    {

        GetComponent<Button>().onClick.AddListener(() =>
        {
            LobbyNetworkHandler.Instance.JoinLobby(lobby);
            //VivoxHandler.Instance.JoinVoiceChannel(lobby.LobbyCode);
            LobbyPanelRelationHandler.Instance.SetLobbyJoinedPanel(true);
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
