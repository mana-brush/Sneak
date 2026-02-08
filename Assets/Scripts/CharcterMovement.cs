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
    
    public StaminaBar staminaBar;
    public float maxStamina = 100;
    public float currentStamina;

    public SoundBar soundBar;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _crouchAction = InputSystem.actions.FindAction("Crouch");
        _sprintAction = InputSystem.actions.FindAction("Sprint");
        _characterController = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
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

        if (movement.x == 0 && movement.z == 0)
        {
            soundBar.SetSoundLevel(0);
        }
        else
        {
            soundBar.SetSoundLevel(2);
        }
        
        if (_sprintAction.inProgress && currentStamina > 0)
        {
            coreMovement *= _sprintSpeedBuff;
            ReduceStamina(.2f);
            soundBar.SetSoundLevel(4);
        } else if (_crouchAction.inProgress && currentStamina > 0)
        {
            coreMovement *= _crouchSpeedDebuff;
            ReduceStamina(.1f);
            soundBar.SetSoundLevel(1);
        }
        else if (currentStamina < maxStamina) // not expending stamina
        {
            RecoverStamina(.1f);
        }
        
        _characterController.Move(coreMovement);
        
    }
    
    void ReduceStamina(float stamina)
    {
        currentStamina -= stamina;
        staminaBar.SetStamina(currentStamina);
    }    
    
    void RecoverStamina(float stamina)
    {
        currentStamina += stamina;
        staminaBar.SetStamina(currentStamina);
    }
    
}
