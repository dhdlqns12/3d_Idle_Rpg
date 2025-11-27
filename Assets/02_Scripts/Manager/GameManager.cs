using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [field: SerializeField]public BigInteger gold;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gold = 1000;
    }

    public void AddGold(BigInteger amount)
    {
        gold += amount;
    }

    public bool ConsumeGold(BigInteger amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            return true;
        }

        return false;
    }

#if UNITY_EDITOR
    [ContextMenu("골드 +1000 추가")]
    private void TestAddGold1K()
    {
        AddGold(1000);
    }

    [ContextMenu("골드 +1M 추가")]
    private void TestAddGold1M()
    {
        AddGold(1000000);
    }

    [ContextMenu("골드 +1B 추가")]
    private void TestAddGold1B()
    {
        AddGold(1000000000);
    }

    [ContextMenu("골드 +1T 추가")]
    private void TestAddGold1T()
    {
        AddGold(BigInteger.Parse("1000000000000"));
    }

    [ContextMenu("소수점 테스트용 골드 추가")]
    private void TestGold()
    {
        AddGold(BigInteger.Parse("1327000000000"));
    }

    [ContextMenu("골드 리셋")]
    private void TestResetGold()
    {
        gold = 0;
        Debug.Log("골드 리셋!");
    }
#endif
}
