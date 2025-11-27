using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float gold;

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

    public void AddGold(float amount)
    {
        gold += amount;
    }

    public bool ConsumeGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            return true;
        }

        return false;
    }
}
