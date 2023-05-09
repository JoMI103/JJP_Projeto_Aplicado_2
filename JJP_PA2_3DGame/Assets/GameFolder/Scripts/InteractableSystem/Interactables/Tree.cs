using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Interactable
{
    [SerializeField] int hitTimes = 10; int hitNow;
    [SerializeField] int quantity;
    [SerializeField] private float respawnTime = 120.0f; 


    private Collider treeCollider;

    private void Start()
    {

        treeCollider = GetComponent<Collider>();
    }

    protected override void Interact()
    {
        PlayerHand ph = PlayerGO.GetComponent<PlayerHand>();

        if(!treeCollider.enabled) return;

        if(ph.activeItem.handId == 5)
        {
            hitNow++;
            if(hitNow >= hitTimes){
                hitNow = 0;
                PlayerStats p = PlayerGO.GetComponent<PlayerStats>();
                p.woodQuantity = p.woodQuantity + quantity;
                treeCollider.enabled = false;
                Transform childTransform = transform.GetChild(0); 
                childTransform.gameObject.SetActive(false);
                Invoke(nameof(Respawn), respawnTime);
            }
        }
    }

    private void Respawn()
    { 
        gameObject.SetActive(true);
        Transform childTransform = transform.GetChild(0);
        childTransform.gameObject.SetActive(true);
        treeCollider.enabled = true;
        
    }
}