using UnityEngine;

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

    /// <summary>
    /// 적이 이동하면 플레이어가 회전 할 수 있으니 공통으로 빼놓음
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="rotationSpeed"></param>
    protected void Rotate(Vector3 direction, float rotationSpeed = 10f)
    {
        if (direction == Vector3.zero) return;

        Transform playerTransform = stateMachine.Player.transform;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 적이 이동하면 플레이어 AttackRange에서 벗어나면 다시 탐색하기 위해 공통으로 빼놓음
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    protected bool IsInAttackRange(Transform target)
    {
        if (target == null) return false;
        if (!target.gameObject.activeSelf) return false;

        float distance = Vector3.Distance(stateMachine.Player.transform.position, target.position);

        return distance <= stateMachine.BaseAttackRange;
    }
}
