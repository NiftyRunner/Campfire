using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Vivox;
using UnityEngine;

public class VivoxHandler : MonoBehaviour
{
    public static VivoxHandler Instance { get; private set; }

    private Dictionary<string, VivoxParticipant> participants = new Dictionary<string, VivoxParticipant>();

    private bool vivoxInitialized;
    private bool vivoxEventsSubscribed;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    private void OnEnable()
    {
        LobbyNetworkHandler.OnJoinedLobby += LobbyNetworkHandler_OnJoinedLobby;
        LobbyNetworkHandler.OnPlayerAuthenticated += LobbyNetworkHandler_OnPlayerAuthenticated;

    }

    private void OnDisable()
    {
        LobbyNetworkHandler.OnJoinedLobby -= LobbyNetworkHandler_OnJoinedLobby;
        LobbyNetworkHandler.OnPlayerAuthenticated -= LobbyNetworkHandler_OnPlayerAuthenticated;

        VivoxService.Instance.ParticipantAddedToChannel -= Instance_ParticipantAddedToChannel;
        VivoxService.Instance.ParticipantRemovedFromChannel -= Instance_ParticipantRemovedFromChannel;
    }

    private void Update()
    {
        foreach (var kvp in participants)
        {
            VivoxParticipant participant = kvp.Value;

            bool isSpeaking = participant.AudioEnergy > 0.01f;

            if (PlayerRegistry.lobbyPlayers.TryGetValue(participant.PlayerId, out var playerGO))
            {
                playerGO.SetMicIcon(isSpeaking);
            }

            // Example: show mic icon
            Debug.Log($"{participant.PlayerId} speaking: {isSpeaking}");
        }
    }

    public async Task InitializeVivoxAsync()
    {
        if (vivoxInitialized)
            return;

        await VivoxService.Instance.InitializeAsync();
        vivoxInitialized = true;

        SubscribeToVivoxEvents();
    }

    private void SubscribeToVivoxEvents()
    {
        if (vivoxEventsSubscribed)
            return;

        VivoxService.Instance.ParticipantAddedToChannel += Instance_ParticipantAddedToChannel;
        VivoxService.Instance.ParticipantRemovedFromChannel += Instance_ParticipantRemovedFromChannel;

        vivoxEventsSubscribed = true;
    }

    public async void LoginToVivoxAsync(string username)
    {
        await InitializeVivoxAsync();

        await VivoxService.Instance.InitializeAsync();


        LoginOptions options = new LoginOptions();

        options.DisplayName = username;
        options.EnableTTS = true;
        await VivoxService.Instance.LoginAsync(options);


        var inputs = VivoxService.Instance.AvailableInputDevices;
        if (inputs.Count > 0)
            await VivoxService.Instance.SetActiveInputDeviceAsync(inputs[0]);

        var outputs = VivoxService.Instance.AvailableOutputDevices;
        if (outputs.Count > 0)
            await VivoxService.Instance.SetActiveOutputDeviceAsync(outputs[0]);

        VivoxService.Instance.UnmuteInputDevice();

        Debug.Log(
                   $"Mic muted: {VivoxService.Instance.IsInputDeviceMuted}"
                   );

        Debug.Log(
                $"Active input: {VivoxService.Instance.ActiveInputDevice?.DeviceName}"
                );
        Debug.Log(
                $"Active input: {VivoxService.Instance.ActiveOutputDevice?.DeviceName}"
                );
    }

    public async void JoinVoiceChannel(string _channelName) 
    {
        Channel3DProperties channelProperties = new Channel3DProperties(
                audibleDistance: 32,
                conversationalDistance: 10,
                audioFadeIntensityByDistanceaudio: 1f,
                audioFadeModel: AudioFadeModel.InverseByDistance
        );


        await VivoxService.Instance.JoinPositionalChannelAsync(_channelName, ChatCapability.TextAndAudio, channelProperties);

        Debug.Log("Joined lobby channel with channel name: " + _channelName);
    }

    private void LobbyNetworkHandler_OnPlayerAuthenticated(string playerName)
    {
        LoginToVivoxAsync(playerName);
    }

    private void LobbyNetworkHandler_OnJoinedLobby(Unity.Services.Lobbies.Models.Lobby lobby)
    {
        JoinVoiceChannel(lobby.Id);
    }

    private void Instance_ParticipantAddedToChannel(VivoxParticipant _participant)
    {
        participants[_participant.PlayerId] = _participant;
        Debug.Log(_participant.PlayerId + " joined voice");
    }

    private void Instance_ParticipantRemovedFromChannel(VivoxParticipant _participant)
    {
        participants.Remove(_participant.PlayerId);
        Debug.Log(_participant.PlayerId + " left voice");
    }
}
