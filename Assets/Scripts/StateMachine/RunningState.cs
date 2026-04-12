using UnityEngine;

public class RunningState : IState
{
    // reference to the player to access its components
    private readonly PlayerController _player;
    
    // called when creating the state
    public RunningState(PlayerController player)
        {
           _player = player;
        }

    public void Enter()
    {
        //Debug.Log("Entering RunningState");
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        //Debug.Log("Exiting RunningState");
    }
}
