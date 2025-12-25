using UnityEngine;
using UnityEngine.UI;

public class LobbyListManager : MonoBehaviour
{
    [Header("Header Button References")]
    [SerializeField] private Button createLobbyPanelButton;
    [SerializeField] private Button refreshLobbyListButton;
    [SerializeField] private Button goToTitleScreenButton;

    [Space]
    [SerializeField] private GameObject listItemPrefab;

    private GameObject titleScreenPanel;
    private GameObject lobbyListScreenPanel;
    private GameObject lobbyCreatePanel;


    private void Start()
    {
        SetUpBackButton();

        refreshLobbyListButton.onClick.AddListener(() =>
        {
            //Reference lobby refresh code
        });

        EnableCreateLobbyPanel();


    }
    private void SetUpBackButton()
    {
        titleScreenPanel = LobbyPanelRelationHandler.Instance.GetMenuPanel();
        lobbyListScreenPanel = LobbyPanelRelationHandler.Instance.GetLobbyPanel();

        goToTitleScreenButton.onClick.AddListener(() =>
        {
            lobbyListScreenPanel.SetActive(false);
            titleScreenPanel.SetActive(true);
        });
    }
    private void EnableCreateLobbyPanel()
    {
        lobbyCreatePanel = LobbyPanelRelationHandler.Instance.GetCreateLobbyPanel();

        createLobbyPanelButton.onClick.AddListener(() =>
        {
            lobbyListScreenPanel.SetActive(false);
            lobbyCreatePanel.SetActive(true);
        });
    }
} 
