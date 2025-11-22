using System;
using UnityEngine;

public class JumpingState : PlayerState
{
    public JumpingState(PlayerController player, PlayerStateFactory states) : base(player, states) { }

    public override void EnterState()
    {
        _player.VerticalVelocity = MathF.Sqrt(_player.JumpHeight * -2 * _player.Gravity);
    }

    public override void UpdateState()
    {
        HandleMovement();
        HandleGravity();
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

    private void HandleGravity()
    {
        _player.VerticalVelocity += _player.Gravity * Time.deltaTime;
    }

    private void HandleStateTransitions()
    {
        // Return to grounded when landing
        if (_player.CharacterController.isGrounded && _player.VerticalVelocity < 0)
        {
            _player.SwitchState(_states.Grounded());
        }
    }
}
