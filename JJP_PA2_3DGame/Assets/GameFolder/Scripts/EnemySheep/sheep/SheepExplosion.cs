using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepExplosion : MonoBehaviour
{
    

    [SerializeField] Transform explosion;
    [SerializeField] private float explosionRadius;
   

      public void Explode(int dmg)
    {
        var surroundingObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var surroundingObject in surroundingObjects)
        {
            PlacedBuilding building = surroundingObject.GetComponent<PlacedBuilding>();
            if (building != null) { building.takeDamge(dmg); }
        }
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public bool debugRadius;

    private void OnDrawGizmos()
    {
        if (debugRadius)
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
