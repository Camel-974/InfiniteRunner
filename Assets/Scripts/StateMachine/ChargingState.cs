using UnityEngine;

public class ChargingState : IState
{
    private readonly PlayerController _player;
    private float _chargerTimer;

    public ChargingState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        //Debug.Log("Entering ChargingState");
        _chargerTimer = 0f;
        _player.Charge();
    }

    public void Update()
    {
        _chargerTimer += Time.deltaTime;

        if (_chargerTimer < _player.ChargeDuration)
            return;
        
        _player.StateMachine.ChangeState(_player.RunningState);
        
    }

    public void Exit()
    {
        //Debug.Log("Exiting ChargingState");
        _chargerTimer = 0f;
    }
}
