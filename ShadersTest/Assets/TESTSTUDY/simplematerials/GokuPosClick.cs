using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GokuPosClick : MonoBehaviour
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
            mousePos.y = mousePos.y / Screen.height;
            {
                Debug.Log("y" + mousePos.y);
                mat.SetFloat("_sliderTest", mousePos.y);
            }
        }
    }
}
