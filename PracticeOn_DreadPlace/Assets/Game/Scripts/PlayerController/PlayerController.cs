using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private const string InputMoveAction = "Player/Move";
    private const string InputJumpAction = "Player/Jump";
    private const string InputLookAction = "Player/Look";

    [Header("Movement")]
    public float Speed;
    private CharacterController _characterController;
    private InputAction _moveAction;

    [Header("Gravity")]
    public float Gravity;
    private float _verticalVelocity;

    [Header("Jump")]
    public float JumpHeight;
    private InputAction _jumpAction;

    [Header("Camera Look")]
    public Transform PlayerCamera;
    public float LookSensitivity;
    public float MaxLookX;
    public float MinLookX;
    private float _camRotationX;
    private InputAction _lookAction;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        _moveAction = InputSystem.actions.FindAction(InputMoveAction);
        _moveAction.Enable();

        _jumpAction = InputSystem.actions.FindAction(InputJumpAction);
        _jumpAction.Enable();

        _lookAction = InputSystem.actions.FindAction(InputLookAction);
        _lookAction.Enable();

        HideCursor();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void LateUpdate()
    {
        HandleLook();
    }

    private void HandleLook()
    {
        Vector2 lookInput = _lookAction.ReadValue<Vector2>();
        Vector2 mouse = lookInput * LookSensitivity;
        transform.Rotate(0, mouse.x, 0);
        _camRotationX -= mouse.y;
        _camRotationX = Mathf.Clamp(_camRotationX, MinLookX, MaxLookX);
        PlayerCamera.localEulerAngles = new Vector3(_camRotationX, 0, 0);
    }

    private void HandleJump()
    {
        if (_characterController.isGrounded && _jumpAction.WasPerformedThisFrame())
            _verticalVelocity = MathF.Sqrt(JumpHeight * -2 * Gravity);
    }

    private void HandleMovement()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();

        Vector3 direction = transform.forward * moveInput.y + transform.right * moveInput.x;
        direction = Vector3.ClampMagnitude(direction, 1f);

        Vector3 move = direction * Speed;

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
}
