using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Point Markers")]
    [SerializeField] private bool showSpawnPointGizmos = true;

    private StageSO currentStage;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private int remainingSpawnCount = 0;
    private bool isSpawning = false;

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

        Debug.Log($"{currentStage.StageName} 시작! (생성할 적: {remainingSpawnCount}마리)");

        StartCoroutine(SpawnRoutine());
    }

    public void ClearAllEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        activeEnemies.Clear();

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

    private IEnumerator SpawnRoutine()
    {
        isSpawning = true;

        for (int i = 0; i < currentStage.SpawnPoints.Length; i++)
        {
            StartCoroutine(SpawnAtPoint(i, currentStage.SpawnPoints[i]));
        }

        while (remainingSpawnCount > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("모든 적 스폰 완료! 남은 스폰: 0");

        yield return StartCoroutine(WaitForAllEnemiesDead());

        OnStageClear();

        isSpawning = false;
    }

    private IEnumerator SpawnAtPoint(int pointIndex, SpawnPointData pointData)
    {
        for (int i = 0; i < pointData.enemyCount; i++)
        {
            GameObject enemy = Instantiate(
                currentStage.EnemyPrefab,
                pointData.position,
                Quaternion.identity
            );

            activeEnemies.Add(enemy);

            remainingSpawnCount--;

            Debug.Log($">SpawnPoint {pointIndex}] 적 생성 ({i + 1}/{pointData.enemyCount}) | 남은 스폰: {remainingSpawnCount}");

            yield return new WaitForSeconds(pointData.spawnInterval);
        }

        Debug.Log($">SpawnPoint {pointIndex} 스폰 완료!");
    }

    private IEnumerator WaitForAllEnemiesDead()
    {
        while (true)
        {
            activeEnemies.RemoveAll(enemy => enemy == null || !enemy.activeSelf);

            if (activeEnemies.Count == 0)
            {
                break;
            }

            Debug.Log($"남은 적: {activeEnemies.Count}, 남은 스폰: {remainingSpawnCount}");

            yield return new WaitForSeconds(1f);
        }
    }

    private void OnStageClear()
    {
        Debug.Log($"스테이지 클리어! (남은 스폰: {remainingSpawnCount}, 남은 적: {activeEnemies.Count})");

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
