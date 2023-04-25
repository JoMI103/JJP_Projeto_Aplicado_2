using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Interactable
{    
    float hp = 100;

    private float respawnTime = 5.0f;
    private Vector3 initialPosition;

    private Collider rockCollider;

    private void Start()
    {
        initialPosition = transform.position;
        rockCollider = GetComponent<Collider>();
    }

    protected override void Interact()
    {
        PlayerHand ph = PlayerGO.GetComponent<PlayerHand>();

        if (ph.activeItem.handId == 6)
        {
            PlayerStats p = PlayerGO.GetComponent<PlayerStats>();
            p.metalQuantity = p.metalQuantity + 250;
            
            hp -= 25; 

            if (hp <= 0)
            {
                rockCollider.enabled = false;
                gameObject.SetActive(false);
                Invoke(nameof(Respawn), respawnTime);
            }
        }
    }

    private void Respawn()
    {
        hp = 100;
        transform.position = initialPosition;
        gameObject.SetActive(true);
        rockCollider.enabled = true;
    }
}