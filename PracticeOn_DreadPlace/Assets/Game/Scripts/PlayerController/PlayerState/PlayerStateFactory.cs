
// State Factory
public class PlayerStateFactory
{
    private PlayerController _player;

    public PlayerStateFactory(PlayerController player)
    {
        _player = player;
    }

    public PlayerState Grounded() => new GroundedState(_player, this);
    public PlayerState Jumping() => new JumpingState(_player, this);
    public PlayerState Crouch() => new CrouchState(_player, this);
    public PlayerState Sprint() => new SprintState(_player, this);
}
