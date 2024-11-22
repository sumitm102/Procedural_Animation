using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;

public class EnvironmentInteractionStateMachine : StateManager<EnvironmentInteractionStateMachine.EEnvironmentInteractionState> // EState Generic
{
    public enum EEnvironmentInteractionState
    {
        Search,
        Approach, 
        Rise,
        Touch,
        Reset,
    }

    private EnvironmentInteractionContext _context;

    [SerializeField] private TwoBoneIKConstraint _leftIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIKConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
    [SerializeField] private CharacterController _controller;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_context != null && _context.ClosestPointOnColliderFromShoulder != null)
        {
            Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, .03f);
        }
    }

    private void Awake()
    {
        ValidateConstraints();

        _context = new EnvironmentInteractionContext(_leftIKConstraint, _rightIKConstraint, _leftMultiRotationConstraint, _rightMultiRotationConstraint, _controller, transform.root);

        InitializeStates();
        ConstructEnvironmentDetectionCollider();
    }

    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftIKConstraint, "Left IK constraint is not assigned.");
        Assert.IsNotNull(_rightIKConstraint, "Right IK constraint is not assigned.");
        Assert.IsNotNull(_leftMultiRotationConstraint, "Left multi-rotation constraint is not assigned.");
        Assert.IsNotNull(_rightMultiRotationConstraint, "Right multi-rotation constraint is not assigned.");
        Assert.IsNotNull(_controller, "Character controller is not assigned.");
    }

    private void InitializeStates()
    {
        States.Add(EEnvironmentInteractionState.Reset, new ResetState(_context, EEnvironmentInteractionState.Reset));
        States.Add(EEnvironmentInteractionState.Rise, new RiseState(_context, EEnvironmentInteractionState.Rise));
        States.Add(EEnvironmentInteractionState.Search, new SearchState(_context, EEnvironmentInteractionState.Search));
        States.Add(EEnvironmentInteractionState.Approach, new ApproachState(_context, EEnvironmentInteractionState.Approach));
        States.Add(EEnvironmentInteractionState.Touch, new TouchState(_context, EEnvironmentInteractionState.Touch));

        currentState = States[EEnvironmentInteractionState.Reset];
    }

    private void ConstructEnvironmentDetectionCollider()
    {
        float wingspan = _controller.height;

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_controller.center.x, _controller.center.y + (.25f * wingspan), _controller.center.z + (.5f * wingspan));
        boxCollider.isTrigger = true;
    }

    
}
