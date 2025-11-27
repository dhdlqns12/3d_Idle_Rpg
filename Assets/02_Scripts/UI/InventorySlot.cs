using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button useButton;

    private Inventory inventoryItem;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        if (useButton != null)
        {
            useButton.onClick.AddListener(OnUseButtonClicked);
        }
    }

    private void UnsubscribeEvents()
    {
        if (useButton != null)
        {
            useButton.onClick.RemoveListener(OnUseButtonClicked);
        }
    }

    public void Setup(Inventory item)
    {
        inventoryItem = item;

        if (iconImage != null && item.itemData.Icon != null)
            iconImage.sprite = item.itemData.Icon;

        if (nameText != null)
            nameText.text = item.itemData.ItemName;

        if (countText != null)
            countText.text = $"x{item.count}";

        if (useButton != null)
        {
            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(OnUseButtonClicked);
        }
    }

    private void OnUseButtonClicked()
    {
        Player player = FindObjectOfType<Player>();

        if (player != null && inventoryItem != null)
        {
            bool success = player.UseItem(inventoryItem.itemData);

            if (success && UIManager.Instance != null)
            {
                UIManager.Instance.UpdateInventoryUI();
            }
        }
    }
}