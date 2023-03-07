using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI screwCount;
    [SerializeField] TextMeshProUGUI gearCount;
    [SerializeField] TextMeshProUGUI wirecableCount;
    public int [] items = new int[3] {0,0,0};

    private void Update()
    {
        screwCount.text = items[0].ToString();
        gearCount.text = items[1].ToString();
        wirecableCount.text = items[2].ToString();
    }

    public void Add(int itemIndex)
    {
        items[itemIndex]++;
    }

    public void Reduce(int itemIndex)
    {
        if(items[itemIndex] >= 0) items[itemIndex]--;
    }


}
