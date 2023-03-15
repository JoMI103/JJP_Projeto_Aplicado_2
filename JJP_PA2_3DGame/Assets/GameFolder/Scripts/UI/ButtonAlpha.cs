using UnityEngine;
using UnityEngine.UI;

public class ButtonAlpha : MonoBehaviour
{
    private void Awake() {
        GetComponent<Image>().alphaHitTestMinimumThreshold= 1.0f;
    }
}
