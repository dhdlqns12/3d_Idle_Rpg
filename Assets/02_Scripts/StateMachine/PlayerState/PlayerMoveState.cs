using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    private struct EnemyDistance // Transform은 숫자값이기 때문에 struct로 선언
    {
        public Transform transform;
        public float distance;
    }


    private float detectionTimer;
    private Collider[] enemyBuffer = new Collider[50]; // 점 감지 후 담을 배열
    private List<EnemyDistance> enemies = new List<EnemyDistance>(); // 적을 가까운 순으로 정렬 할 리스트

    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        detectionTimer = 0f;
        Debug.Log("이동 상태 시작");
    }

    public override void OnStateUpdate()
    {
        detectionTimer += Time.deltaTime;
        if (detectionTimer >= Define.ENEMY_DETECTION_INTERVAL)
        {
            UpdateTarget();
            detectionTimer = 0f;
        }
    }


    public override void OnStatePhysicsUpdate()
    {
        Move();
    }

    public override void Exit()
    {
        Debug.Log("이동 상태 종료");
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        float movementSpd = GetMovementSpeed();

        if (movementDirection != Vector3.zero)
        {
            base.Rotate(movementDirection);
            stateMachine.Player.Controller.Move(movementDirection * movementSpd * Time.fixedDeltaTime);
        }
    }

    private Vector3 GetMovementDirection()
    {
        Transform target = stateMachine.Player.curTarget;

        if (target != null)
        {
            Vector3 direction = (target.position - stateMachine.Player.transform.position).normalized;
            direction.y = 0;
            return direction;
        }
        else
        {
            return stateMachine.Player.transform.forward;
        }
    }

    private float GetMovementSpeed()
    {
        return stateMachine.TotalMovementSpd;
    }

    private void UpdateTarget()
    {
        Transform enemy = FindNearEnemy();

        if (enemy != null)
        {
            stateMachine.Player.SetTarget(enemy);

            if (IsInAttackRange(enemy))
            {
                stateMachine.Player.ToAttackState();
            }
        }
        else
        {
            stateMachine.Player.ClearTarget();
        }
    }

    private Transform FindNearEnemy()
    {
        Vector3 playerPos = stateMachine.Player.transform.position;

        int count = Physics.OverlapSphereNonAlloc(playerPos, stateMachine.BaseDetectionRange, enemyBuffer, stateMachine.Player.EnemyLayer);

        if (count == 0) return null;

        Transform target = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Collider enemyCollider = enemyBuffer[i];

            if (!enemyCollider.gameObject.activeSelf) continue;

            float distance = Vector3.Distance(playerPos, enemyCollider.transform.position);

            enemies.Add(new EnemyDistance { transform = enemyCollider.transform, distance = distance });

            if (distance < minDistance)
            {
                minDistance = distance;
                target = enemyCollider.transform;
            }
        }

        enemies.Sort((a, b) => a.distance.CompareTo(b.distance));

        foreach (var enemy in enemies)
        {
            Vector3 direction = (enemy.transform.position - playerPos).normalized;

            if (Physics.Raycast(playerPos, direction, out RaycastHit hit, enemy.distance, stateMachine.Player.obstacleLayer))
            {
                Debug.Log($"{enemy.transform.name}  장애물 감지:{hit.collider.name}");
                continue;
            }

            Debug.Log($"타겟 설정: {enemy.transform.name} 거리: {enemy.distance:F1})");
            return enemy.transform;
        }

        Debug.Log("모든 적이 장애물 뒤에 있음");
        return null;
    }
}
