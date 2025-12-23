using System;
using TMPro;
using UnityEngine;

public class CustomInputHandler : MonoBehaviour
{
    public static event Action<GameObject> OnInteract;

    //Inspector Fields
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private float interactionDistance;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Transform trackingPointTransform;

    //private SpringJoint joint;
    private Rigidbody heldRigidbody;
    private HoldHandler holdHandler;
    private IInteractable interactable;
    private GameObject hitObject;
    private bool interacting = false;
    
    private void OnEnable()
    {
        CustomInputEvents.OnInteractPerformed += CustomInputEvents_OnInteractPerformed;

        CustomInputEvents.OnHoldStarted += CustomInputEvents_OnHoldStarted;
        CustomInputEvents.OnHoldEnded += CustomInputEvents_OnHoldEnded;
    }


    private void OnDisable()
    {
        CustomInputEvents.OnInteractPerformed -= CustomInputEvents_OnInteractPerformed;

        CustomInputEvents.OnHoldStarted -= CustomInputEvents_OnHoldStarted;
        CustomInputEvents.OnHoldEnded -= CustomInputEvents_OnHoldEnded;
    }

    private void Start()
    {
        //joint = GetComponentInChildren<SpringJoint>();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, hitLayer)) //Raycasting to hit layer
        {
            if (!interacting)
            {
                interacting = true;

                if (interactText != null)
                {
                    interactText.enabled = true;
                    interactText.text = hit.collider.name;
                }

                hitObject = hit.collider.gameObject;
                heldRigidbody = hitObject.GetComponent<Rigidbody>();
                interactable = hitObject.GetComponent<IInteractable>(); //Getting interactable ref
                holdHandler = hitObject.GetComponent<HoldHandler>();
            }
        }
        else
        {
            interacting = false;
            interactText.enabled = false;
            interactText.text = "";
        }

        Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.red);
    }

    //Performing the interact action
    private void CustomInputEvents_OnInteractPerformed()
    {         
        if (interactable != null)
        {
           OnInteract?.Invoke(hitObject); //Notify Systems 
           interactable.Interact(); //Perform object behaviour
        }
    }

    //Setting the tracking transform of player as the transform to follow by object & turning it kinematic
    private void CustomInputEvents_OnHoldStarted()
    {
        if(holdHandler != null)
        {
            trackingPointTransform.position = hitObject.transform.position;

            //joint.connectedBody = heldRigidbody;
            heldRigidbody.isKinematic = true;
            holdHandler.SetTrackingTransform(trackingPointTransform);
            holdHandler.SetHolding(true);
        }
    }

    //Setting hitobject back to non kinematic
    private void CustomInputEvents_OnHoldEnded()
    {
        if(holdHandler != null)
        {
            //joint.connectedBody = null;
            holdHandler.SetHolding(false);
            heldRigidbody.isKinematic = false;
        }
    }
}
