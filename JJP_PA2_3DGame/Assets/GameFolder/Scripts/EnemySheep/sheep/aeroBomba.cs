using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aeroBomba : MonoBehaviour
{
    [SerializeField] Transform explosion;
    [SerializeField] private float explosionRadius;
    private int explosionDmg;
    private LayerMask buildingMask;


    public void SetExplosionStats(int explosionDmg, LayerMask buildingMask)
    {
        this.explosionDmg = explosionDmg;
        this.buildingMask = buildingMask;
    }

    bool dont;

   private void OnTriggerEnter(Collider other)
    {
        if(dont) return;
        dont = true;
        var surroundingObjects = Physics.OverlapSphere(transform.position,explosionRadius, buildingMask);

        foreach(var surroundingObject in surroundingObjects)
        {
            PlacedBuilding placedBuilding = surroundingObject.GetComponent<PlacedBuilding>();
            if (placedBuilding != null) { placedBuilding.takeDamge(explosionDmg); }        
        }
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }

    public bool debugRadius;

    private void OnDrawGizmos()
    {
        if(debugRadius)
        Gizmos.DrawWireSphere(transform.position, explosionRadius); 
    }
    
    
    
 
}
