using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private float attackTimer;
    private float targetCheckTimer;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        attackTimer = 0f;
        targetCheckTimer = 0f;
        Debug.Log("공격 상태 시작");
    }

    public override void OnStateUpdate()
    {
        Transform target = stateMachine.Player.curTarget;
        targetCheckTimer += Time.deltaTime;

        if (targetCheckTimer >= Define.TARGET_CHECK_INTERVEL)
        {
            if (!IsTarget(target))
            {
                stateMachine.Player.ClearTarget();
                stateMachine.Player.ToMoveState();
                return;
            }
            targetCheckTimer = 0f;
        }

        attackTimer += Time.deltaTime;
        float attackSpd = 1f / stateMachine.TotalAtkSpd;

        if (attackTimer >= attackSpd)
        {
            if (target != null)
            {
                Fire(target);
                attackTimer = 0f;
            }
            return;
        }
    }

    public override void OnStatePhysicsUpdate()
    {
        base.OnStatePhysicsUpdate();
    }

    public override void Exit()
    {
        Debug.Log("공격 상태 종료");
    }

    private bool IsTarget(Transform target)
    {
        return target != null && target.gameObject.activeSelf && IsInAttackRange(target);
    }

    private void Fire(Transform target)
    {
        if (stateMachine.Player.bulletPrefab == null)
        {
            Debug.LogWarning("BulletPrefab이 없습니다");
            return;
        }

        GameObject bulletObj = Object.Instantiate(stateMachine.Player.bulletPrefab, stateMachine.Player.fireTransform.position, Quaternion.identity);

        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            Vector3 direction = (target.position - stateMachine.Player.fireTransform.position).normalized;

            float curDamage = stateMachine.TotalAtk;

            bullet.Init(stateMachine.Player.fireTransform.position, direction, curDamage, typeof(Player));

            Debug.Log($"총알 발사 Damage: {curDamage}");
        }
    }
}
