using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerSO playerSO;

    public PlayerBaseState(PlayerStateMachine _stateMachine)
    {
        this.stateMachine = _stateMachine;
        playerSO = stateMachine.Player.Data;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void OnStatePhysicsUpdate()
    {

    }

    public virtual void OnStateUpdate()
    {

    }

    protected void Rotate(Vector3 direction, float rotationSpeed = 10f)
    {
        if (direction == Vector3.zero) return;

        Transform playerTransform = stateMachine.Player.transform;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    protected bool IsInAttackRange(Transform target)
    {
        if (target == null) return false;
        if (!target.gameObject.activeSelf) return false;

        float distance = Vector3.Distance(stateMachine.Player.transform.position, target.position);

        return distance <= stateMachine.BaseAttackRange;
    }
}
