using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    InputManager inputManager;
    [SerializeField] private TextMeshProUGUI interactableMessage;
    [SerializeField] private GameObject selectionWheel;


    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        inputManager = GetComponent<InputManager>();
        inputManager.changeActionMaps.SelectionWheel.started += ctx => activeSelectionWheel();
        inputManager.changeActionMaps.SelectionWheel.canceled += ctx => activeOnFoot();
    }


    public void updateInteractableText(string promptMessage)
    {
        interactableMessage.text = promptMessage;
    }

    public void activeSelectionWheel() {
        selectionWheel.SetActive(true);
  
        Cursor.lockState = CursorLockMode.None;
        inputManager.disableActionMaps();
        inputManager.selectionWheel.Enable();
    }


    public void activeOnFoot()
    {
        selectionWheel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        inputManager.disableActionMaps();
        inputManager.onFoot.Enable();
    }



}
