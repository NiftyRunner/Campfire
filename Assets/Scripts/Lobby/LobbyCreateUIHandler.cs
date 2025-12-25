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

    private GameObject createLobbyPanel;
    private GameObject joinLobbyPanel;

    private void Start()
    {
        createLobbyPanel = LobbyPanelRelationHandler.Instance.GetCreateLobbyPanel();
        joinLobbyPanel = LobbyPanelRelationHandler.Instance.GetLobbyJoinedPanel();

        createLobbyButton.onClick.AddListener(() =>
        {
            SetLobbyValues();
            if (lobbyName == null)
            {
                Debug.Log("LobbyName cannot be null");
                return;
            }

            createLobbyPanel.SetActive(false);
            joinLobbyPanel.SetActive(true);
            LobbyNetworkHandler.Instance.CreateLobby(lobbyName, maxPlayers, isPrivate);
        });

        goToLobbyListButon.onClick.AddListener(() => 
        {
            GameObject lobbyListPanel = LobbyPanelRelationHandler.Instance.GetLobbyPanel();
            

            lobbyListPanel.SetActive(true);
            createLobbyPanel.SetActive(false); //Set CreateLobbyPanelOff
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
