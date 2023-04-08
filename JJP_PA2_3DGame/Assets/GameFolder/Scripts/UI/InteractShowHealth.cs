using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractShowHealth : Interactable
{
    public GameObject UIHealth;

    public override void startLooking()
    {
        UIHealth.SetActive(true);
    }

    public override void stopLooking()
    {
        UIHealth.SetActive(false);
    }
}
