using UnityEngine;

namespace StateMachine
{
    public class StateMachine
    {
        public IState currentState;

        public void ChangeState(IState newState)
        {
            if (currentState == newState)
                return; // Avoid switching to the same state
            
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Execute();
        }
    }
}