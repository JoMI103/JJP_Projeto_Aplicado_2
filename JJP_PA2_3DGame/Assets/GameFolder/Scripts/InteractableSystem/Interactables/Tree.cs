using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Interactable
{
    float hp = 100;

    protected override void Interact()
    {
        PlayerHand ph = PlayerGO.GetComponent<PlayerHand>();

        if(ph.activeItem.handId == 6)
        {
            PlayerStats p = PlayerGO.GetComponent<PlayerStats>();
            p.woodQuantity = p.woodQuantity + 1000;
        }

    }
}