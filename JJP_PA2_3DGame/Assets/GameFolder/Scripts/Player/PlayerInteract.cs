using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask interactableMask;

    private Camera playerCamera;
    private PlayerUI playerUI;
    private InputManager inputManager;

    void Start()
    {
        playerCamera = GetComponent<PlayerLook>().playerCamera;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    Interactable lastInteractable;


    void Update()
    {
        if(lastInteractable != null)
        lastInteractable.stopLooking();
        //Resets Interactable message
        playerUI.updateInteractableText(string.Empty);

        //Ray from camera to distance
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, interactableMask))
        {

            //Gets the gameobject with an Interactable Script with raycast hitinfo
            Interactable interactableAux = hitInfo.collider.GetComponent<Interactable>();
            if (interactableAux != null)
            {
                //updates Interactable promptMessage
                playerUI.updateInteractableText(interactableAux.promptMessage);
                //If interact buttom is pressed calls BaseInteract 
                if (inputManager.onFoot.Interact.triggered) interactableAux.BaseInteract();
            }
        }

        if (Physics.Raycast(ray, out hitInfo, distance * 10, interactableMask))
        {

            //Gets the gameobject with an Interactable Script with raycast hitinfo
            Interactable interactableAux = hitInfo.collider.GetComponent<Interactable>();
            if (interactableAux != null)
            {
                lastInteractable = interactableAux;
                interactableAux.startLooking();
            }
        }
    }
}
