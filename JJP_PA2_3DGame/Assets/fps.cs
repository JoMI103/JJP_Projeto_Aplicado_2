using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fps : MonoBehaviour
{
    public float timer, refresh, avgFramerate;
    string display = "{0} FPS";
    private TextMeshProUGUI m_Text;

    private void Start()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
    }


    private void Update()
    {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0) avgFramerate = (int)(1f / timelapse);
        m_Text.text = string.Format(display, avgFramerate.ToString());
    }
}
