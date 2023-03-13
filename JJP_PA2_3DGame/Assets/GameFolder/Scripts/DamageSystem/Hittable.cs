using UnityEngine;

public class Hittable : MonoBehaviour
{
    //add or remove an DamageEvent Component to this gameobject
    public bool useEvents;

    //this will be called by the player/otherObject gives damage to the object(subclass)
    public void BaseHit(int dmgValue = 0)
    {
        if (useEvents)
            GetComponent<HitEvent>().OnHit.Invoke();
        GiveDamage(dmgValue);
    }

    protected virtual void GiveDamage(int dmgValue)
    {
        //Template function to be overriden by other subclasses
    }
}
