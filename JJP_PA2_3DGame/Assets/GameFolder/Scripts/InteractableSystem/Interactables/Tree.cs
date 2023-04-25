using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Interactable
{
    float hp = 100;

    private float respawnTime = 5.0f; 
    private Vector3 initialPosition;

    private Collider treeCollider;

    private void Start()
    {
        initialPosition = transform.position;
        treeCollider = GetComponent<Collider>();
    }

    protected override void Interact()
    {
        PlayerHand ph = PlayerGO.GetComponent<PlayerHand>();

        if(ph.activeItem.handId == 7)
        {
            PlayerStats p = PlayerGO.GetComponent<PlayerStats>();
            p.woodQuantity = p.woodQuantity + 200;
            
            hp -= 20;

            if (hp <= 0)
            {
                treeCollider.enabled = false;
                Transform childTransform = transform.GetChild(0); 
                childTransform.gameObject.SetActive(false);
                Invoke(nameof(Respawn), respawnTime);
            }
        }
    }

    private void Respawn()
    {
        hp = 100;
        transform.position = initialPosition; 
        gameObject.SetActive(true);
        Transform childTransform = transform.GetChild(0);
        childTransform.gameObject.SetActive(true);
        treeCollider.enabled = true;
        Instantiate(Resources.Load<GameObject>("pCylinder1"), transform);
        
    }
}