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

        Debug.Log($">StageManager {stages.Length}개 스테이지 초기화");

        // TODO: 저장된 데이터 로드
        // LoadStageProgress();
    }

    public void LoadStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stages.Length)
        {
            Debug.LogError($"존재하지 않는 스테이지: {stageIndex + 1}");
            return;
        }

        if (!stageDataList[stageIndex].isUnlocked)
        {
            Debug.LogWarning($"스테이지 {stageIndex + 1}은(는) 아직 잠겨있습니다!");
            return;
        }

        currentStageIndex = stageIndex;

        spawner.ClearAllEnemies();

        spawner.StartStage(stages[currentStageIndex]);

        //if (stageUI != null)
        //{
        //    stageUI.UpdateUI();
        //}

        Debug.Log($"스테이지 {CurrentStageNumber} 로드");
    }

    public void OnStageClear()
    {
        Debug.Log($"스테이지 {CurrentStageNumber} 클리어");

        // 클리어 처리
        stageDataList[currentStageIndex].isCleared = true;

        // 다음 스테이지 해금
        if (currentStageIndex + 1 < stages.Length)
        {
            stageDataList[currentStageIndex + 1].isUnlocked = true;
            Debug.Log($"스테이지 {currentStageIndex + 2} 해금");
        }
        else
        {
            Debug.Log("모든 스테이지 클리어!");
        }

        //if (stageUI != null)
        //{
        //    stageUI.UpdateUI();
        //}
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
