using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // the current active state
    private IState _currentState;
    
    // change to a new state
    public void ChangeState(IState newState)
    {
        // check and exit the current state
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        
        // enter in the new state
        _currentState = newState;
        _currentState.Enter();
    }
    void Update()
    {
        // update the current state
        if (_currentState != null)
        {
            _currentState.Update();
        }
    }
}
