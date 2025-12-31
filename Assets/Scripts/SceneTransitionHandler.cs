using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour
{
    public static SceneTransitionHandler Instance { get; private set; }

    [SerializeField] private Canvas loadingCanvas;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);

        loadingCanvas = GetComponent<Canvas>();
        loadingCanvas.enabled = false;
    }

    private void OnEnable()
    {
        LobbyJoinedUIHandler.OnStartButtonPressed += LobbyJoinedUIHandler_OnStartButtonPressed;
        LobbyNetworkHandler.OnGameStarted += LobbyNetworkHandler_OnGameStarted;
    }

    private void OnDisable()
    {
        LobbyJoinedUIHandler.OnStartButtonPressed -= LobbyJoinedUIHandler_OnStartButtonPressed;
        LobbyNetworkHandler.OnGameStarted -= LobbyNetworkHandler_OnGameStarted;
    }

    private void LobbyJoinedUIHandler_OnStartButtonPressed()
    {
        SceneManager.LoadSceneAsync(1); //Load game scene
        SceneManager.UnloadSceneAsync(0); //Unload menu scene

        loadingCanvas.enabled = true;
    }

    private void LobbyNetworkHandler_OnGameStarted()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        loadingCanvas.enabled = false;
    }
}
