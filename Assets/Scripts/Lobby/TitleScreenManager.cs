using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Connector Reference")]
    [SerializeField] private LobbyPanelRelationHandler relationHandler;

    [Header("Button References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button QuitButton;

    private GameObject menuPanel;
    private GameObject lobbyPanel;

    private void Start()
    {
        menuPanel = relationHandler.GetMenuPanel();
        lobbyPanel = relationHandler.GetLobbyPanel();

        //Activates lobby panel
        playButton.onClick.AddListener(() =>
        {
            menuPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        });

        OptionsButton.onClick.AddListener(() => {
            Debug.Log("Options");
        });

        QuitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
