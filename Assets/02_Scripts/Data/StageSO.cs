using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Game/Stage")]
public class StageSO : ScriptableObject
{
    [field: SerializeField] public int StageNumber { get; private set; } = 1;
    [field: SerializeField] public string StageName { get; private set; } = "Stage_1";

    [field: SerializeField] public GameObject EnemyPrefab { get; private set; }

    [field: SerializeField] public SpawnPointData[] SpawnPoints { get; private set; }
}