using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInputEvents : MonoBehaviour
{
    StarterAssetsClass customInputActions;

    //Interact event
    public static event Action OnInteractPerformed;

    //Hold event
    public static event Action OnHoldStarted;
    public static event Action OnHoldEnded;


    private void Awake()
    {
        //Creating new input action class
        customInputActions = new StarterAssetsClass();
    }

    private void OnEnable()
    {
        customInputActions.Enable();
        
        customInputActions.Player.Interact.performed += Interact_performed; //For interaction

        //Hold mechanics binding
        customInputActions.Player.Hold.started += Hold_started;
        customInputActions.Player.Hold.canceled += Hold_performed;
    }

    private void OnDisable()
    {
        customInputActions.Player.Interact.performed -= Interact_performed;

        customInputActions.Player.Hold.started -= Hold_started;
        customInputActions.Player.Hold.canceled -= Hold_performed;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractPerformed?.Invoke();
    }

    private void Hold_started(InputAction.CallbackContext obj)
    {
        OnHoldStarted?.Invoke();
    }

    private void Hold_performed(InputAction.CallbackContext obj)
    {
        OnHoldEnded?.Invoke();
    }


    //Getters
    StarterAssetsClass GetCustomInputEvents() => customInputActions;
}
