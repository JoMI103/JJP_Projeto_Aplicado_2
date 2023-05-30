using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : ToolInteractable
{    
     [SerializeField] int hitTimes = 2; int hitNow;
    
    [SerializeField] private float respawnTime = 120.0f; 
    
    [SerializeField] private int quantity;
    
    private Collider rockCollider;

    private void Start()
    {
        rockCollider = GetComponent<Collider>();
    }

    protected override void ToolInteract()
    {
        PlayerHand ph = PlayerGO.GetComponent<PlayerHand>();

        if(!rockCollider.enabled) return;
       if(ph.activeItem.handId == 6 && ph.toolsystem.canInteract)
        {
            ph.toolsystem.canInteract = false;
            hitNow++;
            if(hitNow >= hitTimes){
                hitNow = 0;
                PlayerStats p = PlayerGO.GetComponent<PlayerStats>();
                 p.metalQuantity = p.metalQuantity + quantity;
                rockCollider.enabled = false;
                Transform childTransform = transform.GetChild(0); 
                childTransform.gameObject.SetActive(false);
                Invoke(nameof(Respawn), respawnTime);
            }
        }
    }

    private void Respawn()
    {
        Transform childTransform = transform.GetChild(0);
        childTransform.gameObject.SetActive(true);
        rockCollider.enabled = true;
    }
}