using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SheepCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sheepCount;
    [SerializeField] GameObject defeatText;
    private int nSheep = 0;
    void Start()
    {
        defeatText.SetActive(false);
        nSheep = transform.childCount;
    }

    void Update()
    {
        nSheep = transform.childCount;
        sheepCount.text = nSheep.ToString();
        if(nSheep == 0) defeatText.SetActive(true);

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
            Debug.Log("Kill all sheep");
        }
    }


}
