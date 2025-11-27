using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Transform stageButtonContainer;
    [SerializeField] private GameObject stageButtonPrefab;
    [SerializeField] private Button closeButton;

    private void OnEnable()
    {
        SubscribeEvents();
        InitializeStageButtons();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        closeButton?.onClick.AddListener(OnCloseButtonClicked);
    }

    private void UnsubscribeEvents()
    {
        closeButton?.onClick.RemoveAllListeners();
    }

    private void InitializeStageButtons()
    {
        if (StageManager.Instance == null) return;

        foreach (Transform child in stageButtonContainer)
        {
            Destroy(child.gameObject);
        }

        int totalStages = StageManager.Instance.TotalStageCount;

        for (int i = 1; i <= totalStages; i++)
        {
            GameObject buttonObj = Instantiate(stageButtonPrefab, stageButtonContainer);
            StageButton stageButton = buttonObj.GetComponent<StageButton>();
            if (stageButton != null)
            {
                stageButton.Setup(i);
            }
        }
    }

    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
}