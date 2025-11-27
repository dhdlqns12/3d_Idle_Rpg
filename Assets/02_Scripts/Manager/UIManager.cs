using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Gold UI")]
    [SerializeField] private TextMeshProUGUI goldText;

    [Header("Stage UI")]
    [SerializeField] private TextMeshProUGUI stageText;

    [Header("Player HP UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpText;

    [Header("Buttons")]
    [SerializeField] private Button shopButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button stageSelectButton;

    [Header("Shop UI")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform shopItemContainer;
    [SerializeField] private GameObject shopItemSlotPrefab;
    [SerializeField] private Button shopCloseButton;

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform inventoryContainer;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private Button inventoryCloseButton;

    [Header("Stage Select UI")] 
    [SerializeField] private GameObject stageSelectPanel;

    [Header("참조")]
    [SerializeField] private Player player;

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

        shopPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        stageSelectPanel.SetActive(false);

        InitializeShopItems();

        if (hpSlider != null && player != null)
        {
            hpSlider.minValue = 0;
            hpSlider.maxValue = player.maxHP;  
            hpSlider.value = player.curHP;  
        }

        UpdateGoldUI();
        UpdatePlayerHPUI();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }


    private void Update()
    {
        UpdateGoldUI();
        UpdatePlayerHPUI();
    }


    private void SubscribeEvents()
    {
        shopButton?.onClick.AddListener(OnShopButtonClicked);
        inventoryButton?.onClick.AddListener(OnInventoryButtonClicked);
        stageSelectButton?.onClick.AddListener(OnStageSelectButtonClicked);
        shopCloseButton?.onClick.AddListener(OnShopCloseButtonClicked);
        inventoryCloseButton?.onClick.AddListener(OnInventoryCloseButtonClicked);
    }

    private void UnsubscribeEvents()
    {
        shopButton?.onClick.RemoveAllListeners();
        inventoryButton?.onClick.RemoveAllListeners();
        stageSelectButton?.onClick.RemoveAllListeners();
        shopCloseButton?.onClick.RemoveAllListeners();
        inventoryCloseButton?.onClick.RemoveAllListeners();
    }

    public void UpdateGoldUI()
    {
        if (goldText != null && GameManager.Instance != null)
        {
            goldText.text = $"Gold: {NumberFormatter.FormatGold(GameManager.Instance.gold)}";
        }
    }

    private void InitializeShopItems()
    {
        if (ShopManager.Instance == null) return;
        Debug.Log("상점 초기화");

        foreach (Transform child in shopItemContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in ShopManager.Instance.ShopItems)
        {
            GameObject slotObj = Instantiate(shopItemSlotPrefab, shopItemContainer);
            ShopItemSlot slot = slotObj.GetComponent<ShopItemSlot>();
            if (slot != null)
            {
                Debug.Log("상점 슬롯 생성 후 초기화");
                slot.Setup(item);
            }
        }
    }

    public void OnShopButtonClicked()
    {
        bool isActive = !shopPanel.activeSelf;
        shopPanel.SetActive(isActive);
    }

    public void OnShopCloseButtonClicked()
    {
        shopPanel.SetActive(false);
    }

    public void OnInventoryButtonClicked()
    {
        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);

        if (isActive)
        {
            UpdateInventoryUI();
        }
    }

    public void OnInventoryCloseButtonClicked()
    {
        inventoryPanel.SetActive(false);
    }

    public void OnStageSelectButtonClicked()
    {
        bool isActive = !stageSelectPanel.activeSelf;
        stageSelectPanel.SetActive(isActive);
    }

    public void UpdateInventoryUI()
    {
        if (player == null) return;

        foreach (Transform child in inventoryContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var invItem in player.inventory)
        {
            GameObject slotObj = Instantiate(inventorySlotPrefab, inventoryContainer);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
            if (slot != null)
            {
                slot.Setup(invItem);
            }
        }
    }

    public void StageUIUpdate()
    {
        stageText.text = $"Stage: {StageManager.Instance.CurrentStageNumber}";
    }

    public void UpdatePlayerHPUI()
    {
        if (player == null) return;

        if (hpSlider != null)
        {
            hpSlider.value = player.curHP;  
        }

        if (hpText != null)
        {
            hpText.text = $"{player.curHP}/{player.maxHP}";
        }
    }
}