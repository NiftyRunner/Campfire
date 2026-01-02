using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private TMP_InputField playerName;

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

            //Add vivox initialize here
            //Fix player name issue
            LobbyNetworkHandler.Instance.Authenticate(playerName.text);
            //VivoxHandler.Instance.LoginToVivoxAsync(playerName.text);

            LobbyPanelRelationHandler.Instance.SetMenuPanel(false); //Setting title screen off
            LobbyPanelRelationHandler.Instance.SetLobbyPanel(true); //Setting lobby list on

        });

        OptionsButton.onClick.AddListener(() => {
            Debug.Log("Options");
        });

        QuitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
