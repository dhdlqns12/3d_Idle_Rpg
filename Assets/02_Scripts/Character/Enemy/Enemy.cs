using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float hp = 30;

    public void TakeDamage(float damage)
    {
        hp -=damage;

        if(hp<=0)
        {
            Destroy(gameObject);
        }

        Debug.Log($"{hp}");
    }
}
