using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

public class LobbyListManager : MonoBehaviour
{
    [Header("Header Button References")]
    [SerializeField] private Button createLobbyPanelButton;
    [SerializeField] private Button refreshLobbyListButton;
    [SerializeField] private Button goToTitleScreenButton;

    [Space]
    [SerializeField] private Transform listItemTemplate;
    [SerializeField] private Transform container;

    private GameObject titleScreenPanel;
    private GameObject lobbyListScreenPanel;
    private GameObject lobbyCreatePanel;


    private void Awake()
    {
        listItemTemplate.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        LobbyNetworkHandler.OnLobbyListUpdate += LobbyNetworkHandler_OnLobbyListUpdate;
        LobbyNetworkHandler.OnKickedFromLobby += LobbyNetworkHandler_OnKickedFromLobby;
        LobbyNetworkHandler.OnLeftLobby += LobbyNetworkHandler_OnLeftLobby;
    }


    private void OnDisable()
    {
        LobbyNetworkHandler.OnLobbyListUpdate -= LobbyNetworkHandler_OnLobbyListUpdate;
        LobbyNetworkHandler.OnKickedFromLobby -= LobbyNetworkHandler_OnKickedFromLobby;
        LobbyNetworkHandler.OnLeftLobby -= LobbyNetworkHandler_OnLeftLobby;
    }

    private void Start()
    {
        SetUpBackButton();

        refreshLobbyListButton.onClick.AddListener(() =>
        {
            LobbyNetworkHandler.Instance.RefreshLobbyList();
        });

        EnableCreateLobbyPanel();


    }
    private void LobbyNetworkHandler_OnLeftLobby()
    {
        Show();
    }

    private void LobbyNetworkHandler_OnKickedFromLobby(Lobby obj)
    {
        Show();
    }

    private void LobbyNetworkHandler_OnLobbyListUpdate(List<Lobby> _lobbyList)
    {
        UpdateLobbyList(_lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach(Transform child in container)
        {
            if (child == listItemTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach(Lobby lobby in lobbyList)
        {
            Transform listItemSingleTransform = Instantiate(listItemTemplate, container);
            listItemSingleTransform.gameObject.SetActive(true);
            LobbyItemSingleUI singleUI = listItemSingleTransform.GetComponentInChildren<LobbyItemSingleUI>();
            singleUI.UpdateLobby(lobby);
        }
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

    private void Show()
    {
        gameObject.SetActive(true);
    }
} 
