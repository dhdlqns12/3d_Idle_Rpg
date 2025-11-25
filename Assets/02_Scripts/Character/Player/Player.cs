using UnityEngine;

public class Player : MonoBehaviour
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

    public void ToMoveState() => stateMachine.ChangeState(moveState);
    public void ToAttackState() => stateMachine.ChangeState(attackState);

    public void SetTarget(Transform target) => curTarget = target;
    public void ClearTarget() => curTarget = null;

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
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
}
