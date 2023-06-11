using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    public Material mat;
    public Renderer rend;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Clicked();
        }
    }

    void Clicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            hit.collider.gameObject.GetComponent<Renderer>().material.SetFloat("_Valu", 1);
            
        }
    }
    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetButton("Fire1"))
    //    {
    //        Vector3 mousePos = Input.mousePosition;
    //        mousePos.x = mousePos.x / Screen.width;
    //        mousePos.y = mousePos.y / Screen.height;
    //        {
    //            Debug.Log("x" + mousePos.x);
    //            Debug.Log("y" + mousePos.y);
    //            mat.SetFloat("_slideXcenter", mousePos.x);
    //            mat.SetFloat("_slideYcenter", mousePos.y);
    //        }
    //    }
    //}
}
