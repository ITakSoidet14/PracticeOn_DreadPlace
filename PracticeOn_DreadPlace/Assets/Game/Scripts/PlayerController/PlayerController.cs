using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    /*
    private const string InputMoveAction = "Player/Move";
    private const string InputJumpAction = "Player/Jump";
    private const string InputLookAction = "Player/Look";
    private const string InputCrouchAction = "Player/Crouch";
    private const string InputSprintAction = "Player/Sprint";

    [Header("Movement")]
    public float SpeedMovement;
    private CharacterController _characterController;
    private InputAction _moveAction;

    [Header("Gravity")]
    public float Gravity;
    private float _verticalVelocity;

    [Header("Jump")]
    public float JumpHeight;
    private InputAction _jumpAction;

    [Header("Look")]
    public Look CameraLook;

    [Header("Crouch")]
    public bool IsCrouching;
    public float CrouchHeight;
    public float OriginalHeight;
    public float CrouchSpeed;
    public float CrouchTransitionSpeed;
    private InputAction _crouchAction;

    [Header("Sprint")]
    public float SprintSpeed;
    private InputAction _sprintAction;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        _moveAction = InputSystem.actions.FindAction(InputMoveAction);
        _moveAction.Enable();

        _jumpAction = InputSystem.actions.FindAction(InputJumpAction);
        _jumpAction.Enable();

        CameraLook.LookAction = InputSystem.actions.FindAction(InputLookAction);
        CameraLook.LookAction.Enable();

        _crouchAction = InputSystem.actions.FindAction(InputCrouchAction);
        _crouchAction.Enable();

        _sprintAction = InputSystem.actions.FindAction(InputSprintAction);
        _sprintAction.Enable();

        HideCursor();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        CameraLook.HandleLook();
        HandleCrouch();
    }

    private void HandleCrouch()
    {
        if (IsCrouching && CeilCheck()) return;

        if (_crouchAction.triggered)
            IsCrouching = !IsCrouching;

        float targetHeight = IsCrouching ? CrouchHeight : OriginalHeight;
        _characterController.height = Mathf.Lerp(_characterController.height, targetHeight, Time.deltaTime * CrouchTransitionSpeed);
    }

    private bool CeilCheck()
    {
        return Physics.Raycast(transform.position, Vector3.up, _characterController.height + 0.1f);
    }

    private void HandleJump()
    {
        if (_characterController.isGrounded && _jumpAction.WasPerformedThisFrame())
            _verticalVelocity = MathF.Sqrt(JumpHeight * -2 * Gravity);
    }

    private void HandleMovement()
    {
        bool isRunning = _sprintAction.IsPressed();
        float speedMultiplier = isRunning ? SprintSpeed : 1;
        speedMultiplier *= IsCrouching ? CrouchSpeed : 1;

        Vector2 moveInput = _moveAction.ReadValue<Vector2>();

        Vector3 direction = transform.forward * moveInput.y + transform.right * moveInput.x;
        Vector3 moveDirectionClamp = Vector3.ClampMagnitude(direction, 1f);

        Vector3 move = moveDirectionClamp * (SpeedMovement * speedMultiplier);

        CalculateGravity();

        move.y = _verticalVelocity;

        _characterController.Move(move * Time.deltaTime);
    }

    private void CalculateGravity()
    {
        if (_characterController.isGrounded)
        {
            if(_verticalVelocity < 0)
                _verticalVelocity = -2;
        }
        else
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    */

    private const string InputMoveAction = "Player/Move";
    private const string InputJumpAction = "Player/Jump";
    private const string InputLookAction = "Player/Look";
    private const string InputCrouchAction = "Player/Crouch";
    private const string InputSprintAction = "Player/Sprint";

    [Header("Movement")]
    public float SpeedMovement = 5f;
    private CharacterController _characterController;
    private InputAction _moveAction;

    [Header("Gravity")]
    public float Gravity = -9.81f;
    private float _verticalVelocity;

    [Header("Jump")]
    public float JumpHeight = 1.5f;
    private InputAction _jumpAction;

    [Header("Look")]
    public Look CameraLook;

    [Header("Crouch")]
    public float CrouchHeight = 1f;
    public float OriginalHeight = 2f;
    public float CrouchSpeed = 0.5f;
    public float CrouchTransitionSpeed = 5f;
    private InputAction _crouchAction;

    [Header("Sprint")]
    public float SprintSpeed = 1.5f;
    private InputAction _sprintAction;

    // State system
    private PlayerState _currentState;
    private PlayerStateFactory _states;

    // Properties for state access
    public CharacterController CharacterController => _characterController;
    public InputAction MoveAction => _moveAction;
    public InputAction JumpAction => _jumpAction;
    public InputAction CrouchAction => _crouchAction;
    public InputAction SprintAction => _sprintAction;
    public float VerticalVelocity { get => _verticalVelocity; set => _verticalVelocity = value; }
    public bool IsCrouching => _currentState is CrouchState;
    public float CurrentHeight => _characterController.height;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        OriginalHeight = _characterController.height;

        _moveAction = InputSystem.actions.FindAction(InputMoveAction);
        _moveAction.Enable();

        _jumpAction = InputSystem.actions.FindAction(InputJumpAction);
        _jumpAction.Enable();

        CameraLook.LookAction = InputSystem.actions.FindAction(InputLookAction);
        CameraLook.LookAction.Enable();

        _crouchAction = InputSystem.actions.FindAction(InputCrouchAction);
        _crouchAction.Enable();

        _sprintAction = InputSystem.actions.FindAction(InputSprintAction);
        _sprintAction.Enable();

        // Initialize state system
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        HideCursor();
    }

    private void Update()
    {
        _currentState.UpdateState();
        CameraLook.HandleLook();
    }

    public void SwitchState(PlayerState newState)
    {
        _currentState.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }

    public void SetCharacterControllerHeight(float height)
    {
        _characterController.height = height;
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}