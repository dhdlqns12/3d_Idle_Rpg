using System.Collections.Generic;
using UnityEngine;
using static Enums;

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

    public float maxHP;
    public float curHP;

    [Header("버프")]
    public List<ActiveBuff> activeBuffs = new List<ActiveBuff>();

    [Header("인벤토리")]
    public List<Inventory> inventory = new List<Inventory>();

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

    #region 인벤토리 관련
    public void AddItem(ItemSO item, int quantity = 1)
    {
        Inventory existing = inventory.Find(i => i.itemData == item);

        if (existing != null)
        {
            existing.count += quantity;
            Debug.Log($"인벤토리: {item.ItemName} 개수 증가 ({existing.count}개)");
        }
        else
        {
            inventory.Add(new Inventory(item, quantity));
            Debug.Log($"인벤토리: {item.ItemName} 추가 ({quantity}개)");
        }
    }

    public bool UseItem(ItemSO item)
    {
        Inventory inventoryItem = inventory.Find(i => i.itemData == item);

        if (inventoryItem == null || inventoryItem.count <= 0)
        {
            Debug.LogWarning($"아이템이 없습니다: {item.ItemName}");
            return false;
        }

        ApplyItemEffect(item);

        inventoryItem.count--;
        Debug.Log($"아이템 사용: {item.ItemName} (남은 개수: {inventoryItem.count})");

        if (inventoryItem.count <= 0)
        {
            inventory.Remove(inventoryItem);
            Debug.Log($"인벤토리: {item.ItemName} 소진");
        }

        return true;
    }

    private void ApplyItemEffect(ItemSO item)
    {
        switch (item.Effect)
        {
            case ItemEffectType.AttackBoost:
                ApplyBuff(BuffType.Attack, item.Value, item.Duration);
                break;

            case ItemEffectType.AttackSpeedBoost:
                ApplyBuff(BuffType.AttackSpeed, item.Value, item.Duration);
                break;
        }
    }

    public int GetItemCount(ItemSO item)
    {
        Inventory inventoryItem = inventory.Find(i => i.itemData == item);
        return inventoryItem != null ? inventoryItem.count : 0;
    }
    #endregion

    #region 버프 관련
    private void UpdateBuffs()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            activeBuffs[i].remainingTime -= Time.deltaTime;

            if (activeBuffs[i].remainingTime <= 0)
            {
                Debug.Log($"버프 종료: {activeBuffs[i].buffType}");
                activeBuffs.RemoveAt(i);
            }
        }
    }

    public void ApplyBuff(BuffType buffType, float value, float duration)
    {
        activeBuffs.RemoveAll(b => b.buffType == buffType);

        ActiveBuff newBuff = new ActiveBuff(buffType, value, duration);
        activeBuffs.Add(newBuff);

        Debug.Log($"버프 적용: {buffType} +{value}% ({duration}초)");
    }

    public float GetBuffedAttack()
    {
        float baseAtk = stateMachine.TotalAtk;
        float buffPercent = 0f;

        foreach (var buff in activeBuffs)
        {
            if (buff.buffType == BuffType.Attack)
            {
                buffPercent += buff.value;
            }
        }

        return baseAtk * (1f + buffPercent / 100f);
    }

    public float GetBuffedAttackSpeed()
    {
        float baseAtkSpd = stateMachine.TotalAtkSpd;
        float buffPercent = 0f;

        foreach (var buff in activeBuffs)
        {
            if (buff.buffType == BuffType.AttackSpeed)
            {
                buffPercent += buff.value;
            }
        }

        return baseAtkSpd * (1f + buffPercent / 100f);
    }
    #endregion

    private void Die() // 추 후 GameManager로 옮기기
    {
        Debug.Log("플레이어 사망!");
        // TODO: 게임 오버
        Application.Quit();
    }
}
