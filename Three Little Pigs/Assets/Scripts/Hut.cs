using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hut : MonoBehaviour
{
    public float maxHP;
    private float currHP;
    private bool isDestroyed = false;

    [Header("Health Bar")]
    public Image healthBar;
    public Color minHealthColor;
    public Color maxHealthColor;

    private void Start()
    {
        currHP = maxHP;
        healthBar.fillAmount = 1;
        healthBar.color = maxHealthColor;
    }

    public void TakeDamage(float damage)
    {
        if (isDestroyed) return;
        currHP -= damage;
        healthBar.fillAmount = currHP / maxHP;
        healthBar.color = Color.Lerp(minHealthColor, maxHealthColor, healthBar.fillAmount);
        if (currHP <= damage)
        {
            isDestroyed = true;
            healthBar.fillAmount = 0;
            Debug.Log("hut dead");
            GameManager.S.OnHutDestroyed();
            Destroy(this.gameObject, 1.0f);
        }
    }
}
