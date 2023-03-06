using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTimeBomb : MonoBehaviour
{
    public Transform player;
    public float sightRange;
    public LayerMask whatIsPlayer;
    public bool playerInSightRange;
    public float radius, expForce;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (playerInSightRange) Explode();


    }

    private void Explode()
    {
        knockBack();
        GetComponent<Rigidbody>().AddForce(transform.up * 10);
        Destroy(gameObject, 2);
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    void knockBack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            Rigidbody rigb = nearby.GetComponent<Rigidbody>();
            if (rigb != null)
            {
                rigb.AddExplosionForce(expForce, transform.position, radius);
            }
        }
    }
}
