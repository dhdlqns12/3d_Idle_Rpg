using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [field: SerializeField] public PlayerSO Data { get; private set; }

    public PlayerStateMachine stateMachine;
    private PlayerMoveState moveState;
    private PlayerAttackState attackState;

    public CharacterController Controller { get; private set; }

    [field: SerializeField][Tooltip("적 레이어")] public LayerMask EnemyLayer { get; private set; }
    [field: SerializeField][Tooltip("장애물 레이어")] public LayerMask obstacleLayer { get; private set; }

    [field: SerializeField][Tooltip("현재 탐지된 적(디버그 용)")] public Transform curTarget { get; private set; }
    [Tooltip("총알 발사 위치")] public Transform fireTransform { get; private set; }

    [Header("총알 프리팹(임시)")]
    public GameObject bulletPrefab;

    private float maxHP;
    private float curHP;

    public void ToMoveState() => stateMachine.ChangeState(moveState);
    public void ToAttackState() => stateMachine.ChangeState(attackState);

    public void SetTarget(Transform target) => curTarget = target;
    public void ClearTarget() => curTarget = null;

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        maxHP = Data.MaxHP;
        curHP = maxHP;
        fireTransform = transform;
    }

    private void Start()
    {
        stateMachine = new PlayerStateMachine(this);
        moveState = new PlayerMoveState(stateMachine);
        attackState = new PlayerAttackState(stateMachine);

        stateMachine.ChangeState(moveState);
    }

    private void Update()
    {
        stateMachine.OnStateUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.OnStatePhysicsUpdate();
    }

    private bool IsDead()
    {
        return curHP <= 0;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead()) return;

        curHP -= damage;
        curHP = Mathf.Max(curHP, 0);

        Debug.Log($"피격HP: {curHP}/{maxHP} (-{damage})");

        if (IsDead())
        {
            Die();
        }
    }

    private void Die() // 추 후 GameManager로 옮기기
    {
        Debug.Log("플레이어 사망!");
        // TODO: 게임 오버
        Application.Quit();
    }
}
