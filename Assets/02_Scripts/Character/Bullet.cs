using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{

    [Header("발사자 태그 (자동 설정)")]
    private Type ownerType;  // 적끼리는 서로 쏜 총알 맞지 않게

    private float speed;
    private float lifeTime;

    private float damage; // 발사 된 시점에 데미지 고정하기 위해
    private Vector3 direction;
    private float timer;

    public void Init(Vector3 startPosition, Vector3 targetDirection, float _damage, Type _ownerType)
    {
        transform.position = startPosition;
        direction = targetDirection.normalized;
        damage = _damage;
        ownerType = _ownerType;
        timer = 0f;
    }

    private void Awake()
    {
        speed = Define.BULLET_SPEED;
        lifeTime = Define.BULLET_LIFETIME;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ownerType != null && other.GetComponent(ownerType) != null)
        {
            return;
        }

        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            // 발사 시점 데미지 사용
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
