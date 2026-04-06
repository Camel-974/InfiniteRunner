using UnityEngine;

public class MegaChargeState : IState
{
    private readonly PlayerController _player;
    private float _megaChargeTimer;
    
    public MegaChargeState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering MegaCharge State");
        
        // reset timer
        _megaChargeTimer = 0f;
        
        // start the mega charge
        _player.StartMegaCharge();
    }

    public void Update()
    {
        _megaChargeTimer += Time.deltaTime;
        
        // end megaCharge after duration 
        if (_megaChargeTimer < _player.MegaChargeDuration) return;
        
        _player.StateMachine.ChangeState(_player.RunningState);
    }

    public void Exit()
    {
        Debug.Log("Exiting MegaCharge State");
        
        // end the meGACHarge
        _player.EndMegaCharge();
    }
}

