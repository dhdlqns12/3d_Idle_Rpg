using System;
using UnityEngine;

[Serializable]
public class SpawnPointData
{
    [Header("위치")]
    public Vector3 position;

    [Header("스폰 설정")]
    public int enemyCount;
    public float spawnInterval;
}