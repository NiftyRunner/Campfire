using UnityEngine;

public class LobbyPanelRelationHandler : MonoBehaviour
{
    //Used as a connector for scripts to reference panels
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject lobbyJoinedPanel;
    [SerializeField] private GameObject createLobbyPanel;

    public GameObject GetMenuPanel() => menuPanel;

    public GameObject GetLobbyPanel() => lobbyPanel;

    public GameObject GetLobbyJoinedPanel() => lobbyJoinedPanel;

    public GameObject CreateLobbyPanel() => createLobbyPanel;

    private void Start()
    {
        menuPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        lobbyJoinedPanel.SetActive(false);
        createLobbyPanel.SetActive(false);
    }
}
