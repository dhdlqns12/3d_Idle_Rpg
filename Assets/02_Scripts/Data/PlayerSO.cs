using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Player",menuName ="Character/Player")]
public class PlayerSO : ScriptableObject
{
    /// <summary>
    /// 스탯
    /// </summary>
    [field:SerializeField] public float MaxHP { get; private set; }
    [field:SerializeField] public float BaseSpd { get; private set; }
    [field:SerializeField] public float BaseAtk { get; private set; }
    [field:SerializeField] public float BaseAtkSpd { get; private set; }
    [field:SerializeField] public float BaseGetGoldRate { get; private set; }
    [field:SerializeField] public float AttackRange { get; private set; }
    [field:SerializeField] public float DetectionRange { get; private set; }
}
