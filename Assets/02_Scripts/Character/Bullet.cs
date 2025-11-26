using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float lifeTime;

    private Player player; // 데미지 계산 위해 플레이어 참조
    private float fixedDamage; // 발사 된 시점에 데미지 고정하기 위해
    private Vector3 direction;
    private float timer;

    public void Init(Vector3 startPosition, Vector3 targetDirection, float _damage,Player _player)
    {
        player = _player;
        transform.position = startPosition;
        direction = targetDirection.normalized;
        fixedDamage = _damage;
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
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            float damage = player.stateMachine.TotalAtk;
            Debug.Log($"적중 {other.name} {damage} damage!");

            enemy.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
