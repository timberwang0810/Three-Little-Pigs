using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hut : MonoBehaviour
{
    public float maxHP;
    private float currHP;
    private bool isDestroyed = false;

    private void Start()
    {
        currHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (isDestroyed) return;
        currHP -= damage;
        if (currHP <= damage)
        {
            isDestroyed = true;
            Debug.Log("hut dead");
            // TODO: GameManager.S.OnLevelComplete?
            Destroy(this.gameObject, 1.0f);
        }
    }
}
