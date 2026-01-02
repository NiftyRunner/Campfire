using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Qos;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.UI;

public class VivoxTest : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button joinButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        await VivoxService.Instance.InitializeAsync();

        loginButton.onClick.AddListener(() =>
        {
            LoginToVivoxAsync();
        });

        joinButton.onClick.AddListener(() =>
        {
            JoinEchoChannelAsync();
        });
    }

    async void LoginToVivoxAsync()
    {
        LoginOptions options = new LoginOptions();

        options.DisplayName = inputName.text;
        options.EnableTTS = true;
        await VivoxService.Instance.LoginAsync(options);


        var inputs = VivoxService.Instance.AvailableInputDevices;
        if (inputs.Count > 0)
            await VivoxService.Instance.SetActiveInputDeviceAsync(inputs[0]);

        var outputs = VivoxService.Instance.AvailableOutputDevices;
        if (outputs.Count > 0)
            await VivoxService.Instance.SetActiveOutputDeviceAsync(outputs[0]);

        VivoxService.Instance.UnmuteInputDevice();

        Debug.Log("Logged in to vivox ");

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

    async void JoinEchoChannelAsync()
    {
        string channelToJoin = "Lobby";

        await VivoxService.Instance.JoinEchoChannelAsync(channelToJoin, ChatCapability.TextAndAudio);

        Debug.Log("joined echo channel");
    }
}
