using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [field: SerializeField] public EnemySO Data { get; private set; }

    [Header("타겟")]
    [SerializeField] private Transform player;  // 플레이어 참조

    [Header("Fire")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform fireTransform;

    private float curHP;
    private float maxHP;
    private float attackTimer;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        FindPlayer();
    }

    private void Init()
    {
        maxHP = Data.MaxHP;
        curHP = maxHP;
        attackTimer = 0f;

        if (fireTransform == null)
        {
            fireTransform = transform;
        }
    }

    private void FindPlayer()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= Data.AttackRange)
        {
            Fire();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        transform.position += direction * Data.BaseSpd * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void Fire()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        attackTimer += Time.deltaTime;
        float attackInterval = 1f / Data.BaseAtkSpd;

        if (attackTimer >= attackInterval)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    private void Attack()
    {
        if (bulletPrefab != null)
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        GameObject bulletObj = Object.Instantiate(bulletPrefab, fireTransform.position, Quaternion.identity);

        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null && player != null)
        {
            Vector3 direction = (player.position - fireTransform.position).normalized;

            bullet.Init(fireTransform.position, direction, Data.BaseAtk, typeof(Enemy));

            Debug.Log($">Enemy 총알 발사 데미지: {Data.BaseAtk}");
        }
    }

    public void TakeDamage(float damage)
    {
        curHP -= damage;
        curHP = Mathf.Max(curHP, 0);

        Debug.Log($"{gameObject.name} 피격 HP: {curHP}/{maxHP} (-{damage})");

        if (curHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.AddGold(Data.GoldReward);
        Destroy(gameObject);
    }
}
