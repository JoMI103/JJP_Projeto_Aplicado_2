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
            Interactable i = hitInfo.collider.GetComponent<Interactable>();
            if(i!= null){
                playerUI.updateInteractableText(i.promptMessage);
                if (inputManager.onFoot.Interact.triggered) i.BaseInteract(this.gameObject);   
            }else{
                ToolInteractable t = hitInfo.collider.GetComponent<ToolInteractable>();
                if(t != null){
                     playerUI.updateInteractableText(t.promptMessage);
                     if (inputManager.onFoot.PlaceShootAttack.inProgress){ 
                        t.BaseToolInteract(this.gameObject);   
                    }
                    
                }
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
