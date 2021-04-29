using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Debug.Log("damaged: " + damage);
        currHP -= damage;
        UIManager.S.AdjustHealthBar(currHP / maxHP);
        if (currHP <= damage)
        {
            isDestroyed = true;
            UIManager.S.AdjustHealthBar(0);
            Debug.Log("hut dead");
            GameManager.S.OnHutDestroyed();
            Destroy(this.gameObject, 1.0f);
        }
    }
}
