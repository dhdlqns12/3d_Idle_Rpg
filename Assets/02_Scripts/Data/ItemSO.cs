using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item")]
public class ItemSO : ScriptableObject
{
    [field: SerializeField] public string ItemName { get; private set; } = "아이템";
    [field: SerializeField] public string Description { get; private set; } = "설명";
    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: SerializeField] public int Price { get; private set; } = 100;

    [field: SerializeField] public Enums.ItemEffectType Effect { get; private set; }
    [field: SerializeField] public float Value { get; private set; } = 10f;
    [field: SerializeField] public float Duration { get; private set; } = 30f;
}
