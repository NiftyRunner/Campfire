using TMPro;
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
    [SerializeField] private TMP_InputField playerName;

    private GameObject menuPanel;
    private GameObject lobbyPanel;

    private void Awake()
    {
        menuPanel = relationHandler.GetMenuPanel();
        lobbyPanel = relationHandler.GetLobbyPanel();
    }

    private void Start()
    {
        //Activates lobby panel
        playButton.onClick.AddListener(() =>
        {
            if (playerName.text == "")
            {
                Debug.Log("Player Name cannot be null");
                return;
            }

            LobbyNetworkHandler.Instance.Authenticate(playerName.text);
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
