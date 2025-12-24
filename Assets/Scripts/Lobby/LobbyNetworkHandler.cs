using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class LobbyNetworkHandler : MonoBehaviour
{
    private string playerName;
    
    public void SetPlayerNameAndAuthenticate(string _playerName)
    {
        playerName = _playerName;
        Authenticate();
    }


    private void Start()
    {
        
    }

    private async void Authenticate()
    {
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
