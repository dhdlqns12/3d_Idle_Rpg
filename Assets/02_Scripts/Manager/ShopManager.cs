using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Shop Items")]
    [SerializeField] private ItemSO[] shopItems;

    [Header("Player")]
    [SerializeField] private Player player;

    public ItemSO[] ShopItems => shopItems;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }

    public bool PurchaseItem(ItemSO item, int quantity = 1)
    {
        if (player == null)
        {
            Debug.LogError("Player가 없습니다!");
            return false;
        }

        int totalCost = item.Price;

        if (GameManager.Instance.gold < totalCost)
        {
            Debug.LogWarning($"골드 부족필요: {totalCost}, 보유: {GameManager.Instance.gold}");
            return false;
        }

        player.AddItem(item);

        if (!GameManager.Instance.ConsumeGold(totalCost))
        {
            return false;
        }


        Debug.Log($"아이템 구매: {item.ItemName} x{quantity}");

        return true;
    }
}