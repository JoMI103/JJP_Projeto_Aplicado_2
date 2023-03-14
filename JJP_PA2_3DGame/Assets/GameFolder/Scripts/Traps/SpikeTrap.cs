using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int currentDmg;

    [SerializeField] private BoxCollider boxCollider;
    private Animator animator;
    public bool canattack;
    [SerializeField] private LayerMask giveDmg;

    private void Awake()
    {
      
        animator = GetComponent<Animator>();
    }

    

    public void attack()
    {

        RaycastHit[] raycastHit = Physics.BoxCastAll(boxCollider.center+ transform.position, boxCollider.size, Vector3.up, Quaternion.identity, 1, giveDmg);
        foreach (RaycastHit hit in raycastHit)
        {
            Hittable damageAux = hit.collider.GetComponent<Hittable>();
            if (damageAux != null)
                damageAux.BaseHit(currentDmg);
        }

     
    }

    void OnDrawGizmos()
    {
        
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(1,0.2f,0.2f,0.3f);

        Gizmos.DrawCube(boxCollider.center + transform.position, boxCollider.size);
    }

}
