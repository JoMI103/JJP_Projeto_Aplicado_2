using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float radius, expForce;

    void OnCollisionEnter(Collision collision)
    {
        knockBack();

        if (collision.gameObject.tag == "Enemy")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }

        //if tag é ovelha...... dar dano
    }
    // Start is called before the first frame update


    private void Update()
    {
        Destroy(gameObject, 5);
    }

    void knockBack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider nearby in colliders)
        {
            Rigidbody rigb = nearby.GetComponent<Rigidbody>();
            if(rigb != null)
            {
                rigb.AddExplosionForce(expForce, transform.position, radius);
            }
        }
    }
}
