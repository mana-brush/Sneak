using UnityEngine;
using UnityEngine.InputSystem;

public class CharcterMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private InputAction _moveAction;
    private InputAction _crouchAction;
    private InputAction _sprintAction;
    private readonly float _moveSpeed = 5f;
    private readonly float _sprintSpeedBuff = 2f;
    private readonly float _crouchSpeedDebuff = .66f;
    private readonly float gravity = -9.81f;
    private readonly float _downwardForce = -2f; // A small constant downward force
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _crouchAction = InputSystem.actions.FindAction("Crouch");
        _sprintAction = InputSystem.actions.FindAction("Sprint");
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0.0f, moveInput.y);

        
        if (_characterController.isGrounded)
        {
            // Reset vertical velocity when grounded, but apply a constant downward force
            // to keep it stuck to the ground, especially on slopes/stairs.
            if (movement.y < 0)
            {
                movement.y = _downwardForce; 
            }
        }
        else
        {
            // Apply regular gravity when in the air
            movement.y += gravity * Time.deltaTime;
        }
        
        Vector3 coreMovement = movement * (_moveSpeed * Time.deltaTime);
        
        if (_sprintAction.inProgress)
        {
            coreMovement *= _sprintSpeedBuff;
        } else if (_crouchAction.inProgress)
        {
            coreMovement *= _crouchSpeedDebuff;
        }
        
        _characterController.Move(coreMovement);
        
    }
}
