
// Base State class
public abstract class PlayerState
{
    protected PlayerController _player;
    protected PlayerStateFactory _states;

    protected PlayerState(PlayerController player, PlayerStateFactory states)
    {
        _player = player;
        _states = states;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
