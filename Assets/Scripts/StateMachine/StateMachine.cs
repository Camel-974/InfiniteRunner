using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // the current active state
    public IState CurrentState{ get; private set; }
    
    // change to a new state
    public void ChangeState(IState newState)
    {
        // check and exit the current state
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }
        
        // enter in the new state
        CurrentState = newState;
        CurrentState.Enter();
    }
    void Update()
    {
        // update the current state
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}
