using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;


    public void updateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
