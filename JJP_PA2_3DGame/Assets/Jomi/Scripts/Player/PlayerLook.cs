using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera playerCamera;
    private float xRotation;
    [SerializeField] private float xSensitivity = 30.0f, ySensitivity = 30.0f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        //calculate camera rotation
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80.0f, 80.0f);
        //apply to camera
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //rotate player to look left and right with the body
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
