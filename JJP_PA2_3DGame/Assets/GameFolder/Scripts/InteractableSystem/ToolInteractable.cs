using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolInteractable : MonoBehaviour
{
    public string promptMessage;

    protected GameObject PlayerGO;

    //this will be called by the player when he interact with the object(subclass)
    public void BaseToolInteract(GameObject player)
    {
        PlayerGO = player;
 
        ToolInteract();
    }

    protected virtual void ToolInteract()
    {
        //Template function to be overriden by other subclasses
        
    }
}
