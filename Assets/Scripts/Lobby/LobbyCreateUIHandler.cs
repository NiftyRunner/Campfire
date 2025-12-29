using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUIHandler : MonoBehaviour
{
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button goToLobbyListButon;
    [SerializeField] private TMP_InputField LobbyName;
    [SerializeField] private TMP_Dropdown privacyStatusDropdown;

    private string lobbyName;
    private bool isPrivate = false;
    int maxPlayers = 4;

    private void Start()
    {

        createLobbyButton.onClick.AddListener(() =>
        {
            SetLobbyValues();
            if (lobbyName == null)
            {
                Debug.Log("LobbyName cannot be null");
                return;
            }

            LobbyPanelRelationHandler.Instance.SetCreateLobbyPanel(false);
            LobbyPanelRelationHandler.Instance.SetLobbyJoinedPanel(true);

            LobbyNetworkHandler.Instance.CreateLobby(lobbyName, maxPlayers, isPrivate);
        });

        goToLobbyListButon.onClick.AddListener(() => 
        {
            LobbyPanelRelationHandler.Instance.SetLobbyPanel(true);
            LobbyPanelRelationHandler.Instance.SetCreateLobbyPanel(false); //Set CreateLobbyPanelOff
        });
    }

    //Setting Lobby values from player, need to add max player options
    private void SetLobbyValues()
    {
        lobbyName = LobbyName.text;
        int statusValue = privacyStatusDropdown.value;

        switch (statusValue)
        {
            case 0:
                isPrivate = false;
                break;
            case 1:
                isPrivate = true;
                break;
        }
    }
}
