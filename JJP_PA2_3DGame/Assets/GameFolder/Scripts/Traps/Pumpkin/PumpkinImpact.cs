using UnityEngine;

public class PumpkinImpact : MonoBehaviour
{
    [SerializeField] Transform explosion;
    [SerializeField] private float explosionRadius;
    private int explosionDmg;

    public void SetExplosionStats(int explosionDmg)
    {
        this.explosionDmg = explosionDmg;
    }

    bool dont;

    private void OnTriggerEnter(Collider other)
    {
        if(dont) return;
        dont = true;

        var surroundingObjects = Physics.OverlapSphere(transform.position,explosionRadius);

        foreach(var surroundingObject in surroundingObjects)
        {
            EnemySheep enemySheep = surroundingObject.GetComponent<EnemySheep>();
            if (enemySheep != null) { enemySheep.receiveDmg(explosionDmg); }        
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
