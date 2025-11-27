using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Point Debug")]
    [SerializeField] private bool showSpawnPointGizmos = true;

    private StageSO currentStage;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private int remainingSpawnCount = 0;
    private bool isSpawning = false;
    private int currentSpawnID = 0;

    public int RemainingSpawnCount => remainingSpawnCount;
    public int ActiveEnemyCount => activeEnemies.Count;

    public void StartStage(StageSO stage)
    {
        if (isSpawning)
        {
            Debug.LogWarning("이미 스폰 중입니다!");
            return;
        }

        currentStage = stage;

        activeEnemies.Clear();

        remainingSpawnCount = CalculateTotalEnemyCount();

        Debug.Log($"{currentStage.StageName} 시작 (생성할 적: {remainingSpawnCount}마리)");

        StartCoroutine(SpawnRoutine(currentSpawnID));
    }

    public void ClearAllEnemies()
    {
        currentSpawnID++;
        Debug.Log($"EnemySpawner> 스폰 세션 ID: {currentSpawnID} (이전 스폰 무효화)");

        StopAllCoroutines();

        Debug.Log(">EnemySpawner 모든 코루틴 중단");

        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        activeEnemies.Clear();

        isSpawning = false;
        remainingSpawnCount = 0;
        Debug.Log(">Spawner: 모든 적 제거 완료");
    }

    private int CalculateTotalEnemyCount()
    {
        int total = 0;
        foreach (var point in currentStage.SpawnPoints)
        {
            total += point.enemyCount;
        }
        return total;
    }

    private IEnumerator SpawnRoutine(int sessionID)
    {
        isSpawning = true;

        for (int i = 0; i < currentStage.SpawnPoints.Length; i++)
        {
            StartCoroutine(SpawnAtPoint(i, currentStage.SpawnPoints[i], sessionID));
        }

        while (remainingSpawnCount > 0)
        {
            if (sessionID != currentSpawnID)
            {
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }


        yield return StartCoroutine(WaitForAllEnemiesDead(sessionID));

        if (sessionID != currentSpawnID)
        {
            yield break;
        }

        isSpawning = false;
        OnStageClear();
    }

    private IEnumerator SpawnAtPoint(int pointIndex, SpawnPointData pointData, int sessionID)
    {
        for (int i = 0; i < pointData.enemyCount; i++)
        {
            if (sessionID != currentSpawnID)
            {
                yield break;
            }

            GameObject enemy = Instantiate(
                currentStage.EnemyPrefab,
                pointData.position,
                Quaternion.identity
            );

            activeEnemies.Add(enemy);

            remainingSpawnCount--;

            yield return new WaitForSeconds(pointData.spawnInterval);
        }
    }

    private IEnumerator WaitForAllEnemiesDead(int sessionID)
    {
        while (true)
        {
            if (sessionID != currentSpawnID)
            {
                yield break;
            }

            activeEnemies.RemoveAll(enemy => enemy == null || !enemy.activeSelf);

            if (activeEnemies.Count == 0)
            {
                break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void OnStageClear()
    {
        if (StageManager.Instance != null)
        {
            StageManager.Instance.OnStageClear();
        }
    }

    private void OnDrawGizmos()
    {
        if (!showSpawnPointGizmos || currentStage == null) return;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < currentStage.SpawnPoints.Length; i++)
        {
            Vector3 pos = currentStage.SpawnPoints[i].position;

            Gizmos.DrawWireSphere(pos, 1f);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(pos + Vector3.up * 2, $"SpawnPoint {i}\n({currentStage.SpawnPoints[i].enemyCount}마리)");
#endif
        }
    }
}
