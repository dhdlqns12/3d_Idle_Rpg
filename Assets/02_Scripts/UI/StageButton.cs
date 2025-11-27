using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageButton : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button stageBtn;
    [SerializeField] private TextMeshProUGUI stageTxt;
    [Header("Colors")]
    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color clearedColor = new Color(0.5f, 1f, 0.5f);  // 연한 초록색

    private int stageNumber;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    public void Setup(int number)
    {
        stageNumber = number;

        if (stageTxt != null)
        {
            stageTxt.text = $"Stage {stageNumber}";
        }

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (StageManager.Instance == null) return;

        bool isUnlocked = StageManager.Instance.IsStageUnlocked(stageNumber);
        bool isCleared = StageManager.Instance.IsStageClear(stageNumber);

        // 버튼 활성화/비활성화
        if (stageBtn != null)
        {
            stageBtn.interactable = isUnlocked;
        }

        // 색상 변경
        Image buttonImage = stageBtn != null ? stageBtn.GetComponent<Image>() : null;
        if (buttonImage != null)
        {
            if (!isUnlocked)
            {
                buttonImage.color = lockedColor;
            }
            else if (isCleared)
            {
                buttonImage.color = clearedColor;
            }
            else
            {
                buttonImage.color = unlockedColor;
            }
        }

        // 텍스트 표시
        if (stageTxt != null)
        {
            if (!isUnlocked)
            {
                stageTxt.text = $"Stage {stageNumber}\n lock";
            }
            else if (isCleared)
            {
                stageTxt.text = $"Stage {stageNumber}\n clear";
            }
            else
            {
                stageTxt.text = $"Stage {stageNumber}";
            }
        }
    }

    private void SubscribeEvents()
    {
        stageBtn?.onClick.AddListener(OnButtonClicked);
    }

    private void UnsubscribeEvents()
    {
        stageBtn?.onClick.RemoveAllListeners();
    }

    private void OnButtonClicked()
    {
        if (StageManager.Instance != null)
        {
            StageManager.Instance.SelectStage(stageNumber);

            StageSelectUI stageSelectUI = GetComponentInParent<StageSelectUI>();
            if (stageSelectUI != null)
            {
                stageSelectUI.gameObject.SetActive(false);
            }
        }
    }
}