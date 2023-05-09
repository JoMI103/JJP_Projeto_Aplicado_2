using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    
    
    [SerializeField] private float maxSpeed;
    private float speed = 5.0f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 3f;

    private InputManager inputManager;
    private bool sprinting;

    void Start() {
        speed = maxSpeed;
        inputManager = GetComponent<InputManager>();

        //sets de actions 
        inputManager.onFoot.Jump.performed += ctx => Jump();
        inputManager.onFoot.Sprint.performed += ctx => Sprint();
        //inputManager.onFoot.Crouch.performed += ctx => motor.Crouch(); removed
        controller = GetComponent<CharacterController>();    
    }

    private void FixedUpdate()
    {
        //move using the value from our movement action
        ProcessMove(inputManager.onFoot.Movement.ReadValue<Vector2>());
    }

    void Update() {
        //checks if the player is grounded with play character controller
        isGrounded = controller.isGrounded;
    }

    //receive inputs form inputmanager and apply them to our character controller.
    public void ProcessMove(Vector2 input)
    {
        //PlayerInputs
        Vector3 moveDirection = new Vector3(input.x, 0,  input.y);
        //Uses character controller.move to move the player
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        //Gravity
        playerVelocity.y += gravity * Time.deltaTime;

        //If player isGrounded resets fall velocity
        if(isGrounded && playerVelocity.y< 0) 
            playerVelocity.y = -2;

        //Uses character controller.move to simulate Gravity
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (!isGrounded) return;
        playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if(sprinting)  speed = maxSpeed; else speed= maxSpeed * 0.7f;
    }
}


//Crouch

/*
if (lerpCrouch)
{
    crouchTimer += Time.deltaTime;
    float p = crouchTimer / 1; p *= p;

    if (crouching)
        controller.height = Mathf.Lerp(controller.height, 1, p);
    else
        controller.height = Mathf.Lerp(controller.height, 2, p);

    if (p > 1)
    {
        lerpCrouch = true;
        crouchTimer = 0f;
    }
}

public void Crouch()
{
    crouching = !crouching;
    crouchTimer = 0;
    lerpCrouch = true;
}


private bool crouching, lerpCrouch;

private float crouchTimer;

*/