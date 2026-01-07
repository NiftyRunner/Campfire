using TMPro;
using Unity.Services.Lobbies.Models;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Button kickPlayerButton;
    [SerializeField] private GameObject micIcon;

    private Player player;


    private void Awake()
    {
        kickPlayerButton.onClick.AddListener(KickPlayer);
    }

    private void Start()
    {
        micIcon.SetActive(false);
    }

    public void SetKickPlayerButtonVisible(bool visible)
    {
        kickPlayerButton.gameObject.SetActive(visible);
    }

    public void UpdatePlayer(Player player)
    {
        this.player = player;
        playerNameText.text = player.Data[LobbyNetworkHandler.KEY_PLAYER_NAME].Value;

        PlayerRegistry.lobbyPlayers[player.Id] = this;
    }

    private void KickPlayer()
    {
        if (player != null)
        {
            LobbyNetworkHandler.Instance.KickPlayer(player.Id);
        }
    }

    public void SetMicIcon(bool state)
    {
        micIcon.SetActive(state);
    }
}
