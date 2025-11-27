using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [Header("스테이지 데이터(SO)")]
    [SerializeField] private StageSO[] stages;

    [Header("스포너")]
    [SerializeField] private EnemySpawner spawner;

    [Header("UI")]
    // [SerializeField] private StageSelectUI stageUI;

    private List<StageData> stageDataList = new List<StageData>();
    private int currentStageIndex = 0;

    public StageSO CurrentStage => stages[currentStageIndex];
    public int CurrentStageNumber => currentStageIndex + 1;
    public EnemySpawner Spawner => spawner;
    public int TotalStageCount => stages.Length;

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

        InitStageData();
    }

    private void Start()
    {
        LoadStage(currentStageIndex);
    }

    private void InitStageData()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            StageData data = new StageData
            {
                stageNum = i + 1,
                isCleared = false,
                isUnlocked = (i == 0)
            };

            stageDataList.Add(data);
        }

        // TODO: 저장된 데이터 로드
        // LoadStageProgress();
    }

    public void LoadStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stages.Length)
        {
            return;
        }

        if (!stageDataList[stageIndex].isUnlocked)
        {
            return;
        }

        currentStageIndex = stageIndex;

        UIManager.Instance.StageUIUpdate();

        spawner.ClearAllEnemies();

        spawner.StartStage(stages[currentStageIndex]);
    }


    public void OnStageClear()
    {
        stageDataList[currentStageIndex].isCleared = true;

        if (currentStageIndex + 1 < stages.Length)
        {
            stageDataList[currentStageIndex + 1].isUnlocked = true;

            NextStage();
        }
    }

    public void NextStage()
    {
        if (currentStageIndex + 1 < stages.Length)
        {
            LoadStage(currentStageIndex + 1);
        }
        else
        {
            Debug.Log("마지막 스테이지입니다!");
        }
    }

    public void SelectStage(int stageNumber)
    {
        int stageIndex = stageNumber - 1;
        LoadStage(stageIndex);
    }

    public bool IsStageClear(int stageNumber)
    {
        int index = stageNumber - 1;
        if (index < 0 || index >= stageDataList.Count) return false;
        return stageDataList[index].isCleared;
    }

    public bool IsStageUnlocked(int stageNumber)
    {
        int index = stageNumber - 1;
        if (index < 0 || index >= stageDataList.Count) return false;
        return stageDataList[index].isUnlocked;
    }
}
