using Unity.Services.Vivox;
using UnityEngine;

public class VivoxHandler : MonoBehaviour
{
    public static VivoxHandler Instance { get; private set; }

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
    }


    public async void InitializeVivoxAsync()
    {
        //Fix player name issue, meanwhile initialize is in login
    }
    
    public async void LoginToVivoxAsync(string username)
    {
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
        Channel3DProperties channelProperties = new Channel3DProperties();

        channelProperties.AudibleDistance.Equals(32f);
        channelProperties.ConversationalDistance.Equals(10f);
        channelProperties.AudioFadeIntensityByDistance.Equals(1f);


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
}
