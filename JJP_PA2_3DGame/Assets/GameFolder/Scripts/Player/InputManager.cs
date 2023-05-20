using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.SelectionWheelActions selectionWheel;
    public PlayerInput.ChangeActionMapsActions changeActionMaps;
    public PlayerInput.MenuActions Menu;

    private void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        selectionWheel = playerInput.SelectionWheel;
        changeActionMaps = playerInput.ChangeActionMaps;
        Menu = playerInput.Menu;
        DefaultActionMaps();
    }
    private void DefaultActionMaps()
    {
        disableActionMaps();
        onFoot.Enable();
        changeActionMaps.Enable();
    }
    private void OnDisable()
    {
        disableActionMaps();
        changeActionMaps.Disable();
    }
    public void disableActionMaps()
    {
        onFoot.Disable();
        selectionWheel.Disable();
        Menu.Disable();
    }
}
