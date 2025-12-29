using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

public class LobbyListManager : MonoBehaviour
{
    [Header("Header Button References")]
    [SerializeField] private Button createLobbyPanelButton;
    [SerializeField] private Button refreshLobbyListButton;
    [SerializeField] private Button findLobbyByCodeButton;
    [SerializeField] private Button goToTitleScreenButton;

    [Space]
    [SerializeField] private Transform listItemTemplate;
    [SerializeField] private Transform container;

    private void Awake()
    {
        listItemTemplate.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        LobbyNetworkHandler.OnLobbyListUpdate += LobbyNetworkHandler_OnLobbyListUpdate;
        LobbyNetworkHandler.OnKickedFromLobby += LobbyNetworkHandler_OnKickedFromLobby;
        LobbyNetworkHandler.OnLeftLobby += LobbyNetworkHandler_OnLeftLobby;
        LobbyNetworkHandler.OnJoinedLobby += LobbyNetworkHandler_OnJoinedLobby;
    }

    private void OnDisable()
    {
        LobbyNetworkHandler.OnLobbyListUpdate -= LobbyNetworkHandler_OnLobbyListUpdate;
        LobbyNetworkHandler.OnKickedFromLobby -= LobbyNetworkHandler_OnKickedFromLobby;
        LobbyNetworkHandler.OnLeftLobby -= LobbyNetworkHandler_OnLeftLobby;
        LobbyNetworkHandler.OnJoinedLobby -= LobbyNetworkHandler_OnJoinedLobby;
    }

    private void Start()
    {
        SetUpBackButton();


        refreshLobbyListButton.onClick.AddListener(() =>
        {
            LobbyNetworkHandler.Instance.RefreshLobbyList();
        });

        findLobbyByCodeButton.onClick.AddListener(() => {
            LobbyPanelRelationHandler.Instance.SetFindLobbyPanel(true);
            gameObject.SetActive(false);
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

    private void LobbyNetworkHandler_OnJoinedLobby(Lobby obj)
    {
        gameObject.SetActive(true);
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

        goToTitleScreenButton.onClick.AddListener(() =>
        {
            LobbyPanelRelationHandler.Instance.SetLobbyPanel(false);
            LobbyPanelRelationHandler.Instance.SetMenuPanel(true); //Title screen panel
        });
    }
    private void EnableCreateLobbyPanel()
    {

        createLobbyPanelButton.onClick.AddListener(() =>
        {
            LobbyPanelRelationHandler.Instance.SetLobbyPanel(false); //Lobby list panel
            LobbyPanelRelationHandler.Instance.SetCreateLobbyPanel(true); //create lobby panel

        });
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
} 
