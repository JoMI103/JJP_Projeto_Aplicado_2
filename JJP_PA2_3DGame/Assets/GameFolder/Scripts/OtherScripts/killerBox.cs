using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killerBox : MonoBehaviour
{
    [SerializeField] private int currentDmg;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private LayerMask giveDmg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] raycastHit = Physics.BoxCastAll(boxCollider.center + transform.position - new Vector3(0,transform.localScale.y/2,0),transform.localScale / 2, Vector3.up, Quaternion.identity, 0, giveDmg);

        foreach (RaycastHit hit in raycastHit)
        { 
            Hittable damageAux = hit.collider.GetComponent<Hittable>();
            if (damageAux != null)
            {
              
                damageAux.BaseHit(currentDmg);
            }
        }
        
      
    }

    void OnDrawGizmos()
    {

        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(1,0.2f,0.2f,0.3f);

        Gizmos.DrawCube(boxCollider.center + transform.position, transform.localScale);
    }
}
