using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTrees : MonoBehaviour
{
    public float minChopTime = 3.0f; 
    public float maxChopTime = 7.0f; 
    public float regrowTime = 15.0f; 
    public int woodAmount = 10; 
    public GameObject woodPrefab; 

    private bool canChop = false; 
    private bool isChopping = false; 

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canChop = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canChop = false;
        }
    }

    void Update()
    {
        if (canChop && Input.GetKeyDown(KeyCode.F) && !isChopping)
        {
            StartCoroutine(ChopTree());
        }
    }

    IEnumerator ChopTree()
    {
        isChopping = true;
        yield return new WaitForSeconds(Random.Range(minChopTime, maxChopTime));
        
        Destroy(gameObject); 
        isChopping = false;
        yield return new WaitForSeconds(Random.Range(regrowTime - 5.0f, regrowTime)); 
        Instantiate(gameObject, transform.position, transform.rotation); 
    }
}