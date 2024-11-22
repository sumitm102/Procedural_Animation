using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvironmentInteractionState : BaseState<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    protected EnvironmentInteractionContext context;

    public EnvironmentInteractionState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState stateKey) : base(stateKey)
    {
        this.context = context;
    }

    private Vector3 GetClosestPointOnCollider(Collider intersectingCollider, Vector3 positionCheck)
    {
        return intersectingCollider.ClosestPoint(positionCheck);
    }

    protected void StartIKTargetPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider.gameObject.layer == LayerMask.NameToLayer("Interactable") && context.CurrentIntersectingCollider == null)
        {
            context.CurrentIntersectingCollider = intersectingCollider;
            Vector3 closestPointFromRoot = GetClosestPointOnCollider(intersectingCollider, context.RootTransform.position);
            context.SetCurrentSide(closestPointFromRoot);

            SetIKTargetPosition();
        }

        
    }

    protected void UpdateIKTargetPosition(Collider intersectingCollider)
    {
        if(intersectingCollider == context.CurrentIntersectingCollider)
        {
            SetIKTargetPosition();
        }
    }

    protected void ResetIKTargetPositionTracking(Collider intersectingCollider)
    {
        if(intersectingCollider == context.CurrentIntersectingCollider)
        {
            context.CurrentIntersectingCollider = null;
            context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        }
    }

    private void SetIKTargetPosition()
    {
        context.ClosestPointOnColliderFromShoulder = GetClosestPointOnCollider(context.CurrentIntersectingCollider, new Vector3(context.CurrentShoulderTransform.position.x, 
            context.CharacterShoulderHeight, context.CurrentShoulderTransform.position.z));

        Vector3 rayDirection = context.CurrentShoulderTransform.position - context.ClosestPointOnColliderFromShoulder;
        Vector3 normalizedRayDirection = rayDirection.normalized;
        float offsetDistance = .05f;
        Vector3 offset = normalizedRayDirection * offsetDistance;

        Vector3 offsetPosition = context.ClosestPointOnColliderFromShoulder + offset;

        context.CurrentIKTargetTransform.position = offsetPosition;
    }
}
