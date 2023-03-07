using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera playerCamera;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask interactableMask;
    private PlayerUI playerUI;

    private InputManager inputManager;

    void Start()
    {
        playerCamera = GetComponent<PlayerLook>().playerCamera;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    
    void Update()
    {
        playerUI.updateText(string.Empty);

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, interactableMask)) 
        {
            Interactable interactableAux = hitInfo.collider.GetComponent<Interactable>();
            if (interactableAux != null) { 
                playerUI.updateText(interactableAux.promptMessage);
                if (inputManager.onFoot.Interact.triggered)
                {
                    interactableAux.BaseInteract();
                }
            
            }
        }

    }
}
