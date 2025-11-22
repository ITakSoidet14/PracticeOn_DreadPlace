using UnityEngine;
// Concrete States
public class GroundedState : PlayerState
{
    public GroundedState(PlayerController player, PlayerStateFactory states) : base(player, states) { }

    public override void EnterState()
    {
        // Reset vertical velocity when grounded
        if (_player.VerticalVelocity < 0)
            _player.VerticalVelocity = -2f;
    }

    public override void UpdateState()
    {
        HandleMovement();
        HandleStateTransitions();
    }

    public override void ExitState() { }

    private void HandleMovement()
    {
        Vector2 moveInput = _player.MoveAction.ReadValue<Vector2>();
        Vector3 direction = _player.transform.forward * moveInput.y + _player.transform.right * moveInput.x;
        Vector3 moveDirectionClamp = Vector3.ClampMagnitude(direction, 1f);

        Vector3 move = moveDirectionClamp * _player.SpeedMovement;
        move.y = _player.VerticalVelocity;

        _player.CharacterController.Move(move * Time.deltaTime);
    }

    private void HandleStateTransitions()
    {
        // Check for jump
        if (_player.JumpAction.WasPerformedThisFrame())
        {
            _player.SwitchState(_states.Jumping());
            return;
        }

        // Check for crouch
        if (_player.CrouchAction.WasPerformedThisFrame())
        {
            _player.SwitchState(_states.Crouch());
            return;
        }

        // Check for sprint
        if (_player.SprintAction.IsPressed() && _player.MoveAction.ReadValue<Vector2>().y > 0)
        {
            _player.SwitchState(_states.Sprint());
            return;
        }

        // Apply gravity
        if (!_player.CharacterController.isGrounded)
        {
            _player.VerticalVelocity += _player.Gravity * Time.deltaTime;
        }
    }
}
