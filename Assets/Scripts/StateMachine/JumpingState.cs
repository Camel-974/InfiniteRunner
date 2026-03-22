using UnityEngine;

public class JumpingState : IState
{
    private readonly PlayerController _player;

    public JumpingState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering JumpingState");
        _player.Jump();
    }

    public void Update()
    {
        if (_player.IsGrounded())
        {
            _player.StateMachine.ChangeState(_player.RunningState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting JumpingState");
    }
}
