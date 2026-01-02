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
    }

    private void OnDisable()
    {
        LobbyNetworkHandler.OnJoinedLobby -= LobbyNetworkHandler_OnJoinedLobby;
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
    }

    public async void JoinVoiceChannel(string _channelName) 
    {
        Channel3DProperties channelProperties = new Channel3DProperties();
        //channelProperties.AudioFadeIntensityByDistance.Equals(10);

        await VivoxService.Instance.JoinPositionalChannelAsync(_channelName, ChatCapability.TextAndAudio, channelProperties);

        Debug.Log("Joined lobby channel with channel name: " + _channelName);
    }

    private void LobbyNetworkHandler_OnJoinedLobby(Unity.Services.Lobbies.Models.Lobby lobby)
    {
        JoinVoiceChannel(lobby.LobbyCode);
    }
}
