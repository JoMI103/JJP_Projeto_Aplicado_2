using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    InputManager inputManager;
    [SerializeField] private TextMeshProUGUI interactableMessage;
    [SerializeField] private GameObject selectionWheel;
    [SerializeField] private GameObject pauseMenu;


    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        inputManager = GetComponent<InputManager>();
        
        inputManager.changeActionMaps.SelectionWheel.started += ctx => activeSelectionWheel();
        inputManager.changeActionMaps.SelectionWheel.canceled += ctx => activeOnFoot();
        
        inputManager.changeActionMaps.PauseMenu.started += ctx => PauseUnpause();
        
    }


    public void updateInteractableText(string promptMessage)
    {
        interactableMessage.text = promptMessage;
    }

    public void activeSelectionWheel() {
        turnOffUI();
        selectionWheel.SetActive(true);
    
        Cursor.lockState = CursorLockMode.None;
        inputManager.disableActionMaps();
        inputManager.selectionWheel.Enable();
    }


    public void activeOnFoot()
    {
        turnOffUI();
        Cursor.lockState = CursorLockMode.Locked;
        inputManager.disableActionMaps();
        inputManager.onFoot.Enable();
    }

    public void PauseUnpause(){
        
        if(Time.timeScale == 0){
            activeOnFoot();
            Time.timeScale = 1;
        }else{
            turnOffUI();
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            inputManager.disableActionMaps();
            inputManager.Menu.Enable();
            Time.timeScale = 0;
        }
    }
    
    private void turnOffUI(){
        selectionWheel.SetActive(false);
        pauseMenu.SetActive(false);
    }


}
