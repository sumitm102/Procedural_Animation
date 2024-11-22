using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetState : EnvironmentInteractionState
{
    private float _elaspedTime = 0f;
    private float _resetDuration = 2f;

    public ResetState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Entering Reset State");
        _elaspedTime = 0f;
        context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        context.CurrentIntersectingCollider = null;
    }
    public override void UpdatState()
    {
        Debug.Log("Updating Reset State");
        _elaspedTime += Time.deltaTime;
    }

    public override void ExitState()
    {

    }

    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        
        bool isMoving = true; //Change this

        if (_elaspedTime >= _resetDuration && isMoving) return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Search;

        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
        StartIKTargetPositionTracking(other);
    }

    public override void OnTriggerExit(Collider other)
    {

    }

    public override void OnTriggerStay(Collider other)
    {

    }
}
