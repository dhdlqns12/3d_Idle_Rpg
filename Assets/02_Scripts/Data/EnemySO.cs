using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy",menuName = "Character/Enemy")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public float MaxHP { get; private set; }
    [field: SerializeField] public float BaseSpd { get; private set; }
    [field: SerializeField] public float BaseAtk { get; private set; }
    [field: SerializeField] public float BaseAtkSpd { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [SerializeField] private BigIntegerSerializable goldReward = new BigIntegerSerializable("10");

    public BigInteger GoldReward => goldReward.Value;
}
