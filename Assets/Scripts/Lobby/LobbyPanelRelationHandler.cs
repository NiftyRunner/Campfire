using UnityEngine;

public class LobbyPanelRelationHandler : MonoBehaviour
{
    public static LobbyPanelRelationHandler Instance { get; private set; }

    //Used as a connector for scripts to reference panels
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject lobbyJoinedPanel;
    [SerializeField] private GameObject createLobbyPanel;
    [SerializeField] private GameObject findLobbyPanel;

    public void SetMenuPanel(bool state) => menuPanel.SetActive(state);

    public void SetLobbyPanel(bool state) => lobbyPanel.SetActive(state);

    public void SetLobbyJoinedPanel(bool state) => lobbyJoinedPanel.SetActive(state);

    public void SetCreateLobbyPanel(bool state) => createLobbyPanel.SetActive(state);

    public void SetFindLobbyPanel(bool state) => findLobbyPanel.SetActive(state);

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
        findLobbyPanel.SetActive(false);
    }
}
