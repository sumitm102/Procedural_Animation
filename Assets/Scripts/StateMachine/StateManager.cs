using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//Brain or Central Controller of a StateMachine
public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> currentState;

    protected bool isTransitioningState = false; 

    private void Start() 
    {
        currentState.EnterState();
    }

    private void Update() 
    {
        EState nextStateKey = currentState.GetNextState();

        if (!isTransitioningState && nextStateKey.Equals(currentState.StateKey))
        {
            currentState.UpdatState();
        }
        else if(!isTransitioningState)
        {
            TransitionToNextState(nextStateKey);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        currentState.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other) 
    {
        currentState.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other) 
    {
        currentState.OnTriggerExit(other);
    }

    protected void TransitionToNextState(EState stateKey)
    {
        isTransitioningState = true;

        currentState?.ExitState();

        currentState = States[stateKey];

        currentState?.EnterState();

        isTransitioningState = false;
    }
}
