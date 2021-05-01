using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hut : MonoBehaviour
{
    public float maxHP;
    public int maxPigs;
    private float currHP;
    private int currPigs = 0;
    private bool isDestroyed = false;

    private void Start()
    {
        currHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (isDestroyed) return;
        currHP -= damage;
        UIManager.S.AdjustHealthBar(currHP / maxHP);
        if (currHP <= damage)
        {
            isDestroyed = true;
            UIManager.S.AdjustHealthBar(0);
            GameManager.S.OnHutDestroyed();
            Destroy(this.gameObject, 1.0f);
        }
    }

    public void OnPigEntered()
    {
        currPigs++;
        if (currPigs == maxPigs) GameManager.S.ResetLevel();
    }

}
