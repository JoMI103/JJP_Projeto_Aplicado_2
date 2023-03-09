using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactableMessage;


    public void updateInteractableText(string promptMessage)
    {
        interactableMessage.text = promptMessage;
    }
}
