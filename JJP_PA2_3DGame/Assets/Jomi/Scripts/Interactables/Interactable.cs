using UnityEngine;

public abstract class  Interactable : MonoBehaviour
{
    //add or remove an InteractionEvent Component to this gameobject
    public bool useEvents; 

    //Message displayed to player when looking at an interactable
    public string promptMessage;

    //this will be called by the player when he interact with the object(subclass)
    public void BaseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact();
    }

    protected virtual void Interact()
    {
        //Template function to be overriden by other subclasses
    }

}
