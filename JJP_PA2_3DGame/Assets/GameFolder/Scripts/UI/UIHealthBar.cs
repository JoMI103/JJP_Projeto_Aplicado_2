using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Image foreGroundImage,backGroundImage;
    public Vector3 offset;

 
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 direction = (target.position - Camera.main.transform.position).normalized;
        bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;
        foreGroundImage.enabled = !isBehind;
        backGroundImage.enabled = !isBehind;
        transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }

    public void SetHealthBarPercentage(float percentage)
    {
       
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth* percentage;
        foreGroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

    }
}
