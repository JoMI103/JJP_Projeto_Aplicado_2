using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.x = mousePos.x / Screen.width;
            mousePos.y = mousePos.y / Screen.height;
            {
                Debug.Log("x" + mousePos.x);
                Debug.Log("y" + mousePos.y);
                mat.SetFloat("_slideXcenter", mousePos.x);
                mat.SetFloat("_slideYcenter", mousePos.y);
            }
        }
    }
}
