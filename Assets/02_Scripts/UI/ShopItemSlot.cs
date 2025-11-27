using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlot : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button buyButton;

    private ItemSO itemData;

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
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }
    }

    private void UnsubscribeEvents()
    {
        if (buyButton != null)
        {
            buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        }
    }

    public void Setup(ItemSO item)
    {
        itemData = item;

        Debug.Log($">ShopItemSlot Setup: {item.ItemName}, Price: {item.Price}");

        if (iconImage != null && item.Icon != null)
            iconImage.sprite = item.Icon;

        if (nameText != null)
            nameText.text = item.ItemName;

        if (priceText != null)
            priceText.text = $"{item.Price}G";

        if (descriptionText != null)
            descriptionText.text = item.Description;
    }

    private void OnBuyButtonClicked()
    {
        if (ShopManager.Instance != null && itemData != null)
        {
            bool success = ShopManager.Instance.PurchaseItem(itemData);

            if (success && UIManager.Instance != null)
            {
                UIManager.Instance.UpdateInventoryUI();
            }
        }
    }
}
