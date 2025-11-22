using UnityEngine;

public class CrouchState : PlayerState
{
    private float _targetHeight;
    private bool _isTransitioning;

    public CrouchState(PlayerController player, PlayerStateFactory states) : base(player, states) { }

    public override void EnterState()
    {
        _targetHeight = _player.CrouchHeight;
        _isTransitioning = true;
    }

    public override void UpdateState()
    {
        HandleCrouchTransition();
        HandleMovement();
        HandleStateTransitions();
    }

    public override void ExitState()
    {
        _targetHeight = _player.OriginalHeight;
        _isTransitioning = true;
        // Immediately set the target height when exiting crouch to avoid getting stuck
        _player.SetCharacterControllerHeight(_player.OriginalHeight);
    }

    private void HandleCrouchTransition()
    {
        if (_isTransitioning)
        {
            float newHeight = Mathf.Lerp(
                _player.CurrentHeight,
                _targetHeight,
                Time.deltaTime * _player.CrouchTransitionSpeed
            );

            _player.SetCharacterControllerHeight(newHeight);

            // Check if transition is complete
            if (Mathf.Abs(_player.CurrentHeight - _targetHeight) < 0.01f)
            {
                _player.SetCharacterControllerHeight(_targetHeight);
                _isTransitioning = false;
            }
        }
    }

    private void HandleMovement()
    {
        Vector2 moveInput = _player.MoveAction.ReadValue<Vector2>();
        Vector3 direction = _player.transform.forward * moveInput.y + _player.transform.right * moveInput.x;
        Vector3 moveDirectionClamp = Vector3.ClampMagnitude(direction, 1f);

        Vector3 move = moveDirectionClamp * (_player.SpeedMovement * _player.CrouchSpeed);
        move.y = _player.VerticalVelocity;

        _player.CharacterController.Move(move * Time.deltaTime);
    }

    private void HandleStateTransitions()
    {
        // Check for uncrouch - теперь провер€ем нажатие кнопки приседани€ дл€ выхода
        if (_player.CrouchAction.WasPerformedThisFrame())
        {
            // ѕровер€ем, нет ли преп€тствий над головой
            if (!CeilCheck())
            {
                _player.SwitchState(_states.Grounded());
            }
            return;
        }

        // Apply gravity
        if (!_player.CharacterController.isGrounded)
        {
            _player.VerticalVelocity += _player.Gravity * Time.deltaTime;
        }
        else if (_player.VerticalVelocity < 0)
        {
            _player.VerticalVelocity = -2f;
        }
    }

    private bool CeilCheck()
    {
        float raycastDistance = (_player.OriginalHeight - _player.CurrentHeight) + 0.2f;
        return Physics.Raycast(_player.transform.position, Vector3.up, raycastDistance);
    }
}
