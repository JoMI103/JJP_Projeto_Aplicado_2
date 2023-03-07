using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDrop : MonoBehaviour
{
    [SerializeField] private int itemIndex;
    private GameObject itmmng;

    private void Start()
    {
        itmmng = GameObject.FindWithTag("ItemManager");
    }



    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(collision.gameObject);
            itmmng.GetComponent<ItemManager>().Add(itemIndex);
            Destroy(gameObject);
        }
    }


}
