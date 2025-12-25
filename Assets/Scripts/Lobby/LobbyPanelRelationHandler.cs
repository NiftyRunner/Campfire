using UnityEngine;

public class LobbyPanelRelationHandler : MonoBehaviour
{
    public static LobbyPanelRelationHandler Instance { get; private set; }

    //Used as a connector for scripts to reference panels
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject lobbyJoinedPanel;
    [SerializeField] private GameObject createLobbyPanel;

    public GameObject GetMenuPanel() => menuPanel;

    public GameObject GetLobbyPanel() => lobbyPanel;

    public GameObject GetLobbyJoinedPanel() => lobbyJoinedPanel;

    public GameObject GetCreateLobbyPanel() => createLobbyPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        menuPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        lobbyJoinedPanel.SetActive(false);
        createLobbyPanel.SetActive(false);
    }
}
