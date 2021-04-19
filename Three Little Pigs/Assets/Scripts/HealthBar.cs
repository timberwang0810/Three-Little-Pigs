using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Color minHealthColor;
    public Color maxHealthColor;
    public SpriteRenderer fill;
    private void Start()
    {
        fill.transform.localScale = new Vector3(1, 1, 1);
        fill.color = maxHealthColor;
    }

    public void AdjustFillAmount(float currHP, float maxHP)
    {
        fill.transform.localScale = new Vector3(currHP / maxHP, 1, 1);
        fill.color = Color.Lerp(minHealthColor, maxHealthColor, currHP / maxHP);
    }
}
