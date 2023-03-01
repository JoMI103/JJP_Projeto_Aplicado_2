using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    private PlayerUI playerUI;
    private InputManager inputManager;

    private Camera playerCamera;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask interactableMask;

    void Start()
    {
        playerCamera = GetComponent<PlayerLook>().playerCamera;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }
}
