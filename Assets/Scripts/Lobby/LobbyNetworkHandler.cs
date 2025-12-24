using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class LobbyNetworkHandler : MonoBehaviour
{
    private string playerName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        

        //await AuthenticationService.Instance.
    }

    private async void Authenticate(string _playerName)
    {
        this.playerName = _playerName;
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);

        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () =>
        {
            
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

}
