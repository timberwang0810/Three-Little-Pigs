using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PreparationTime : MonoBehaviour
{
    public int prepTime;
    private float timeLeft;
    public TextMeshProUGUI timeText;

    public GameObject[] spawners;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = (float) prepTime;  
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState != GameManager.GameState.playing) return;

        if (timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
            timeText.text = (Mathf.Ceil(timeLeft)).ToString();
            if (timeLeft < 0)
            {
                for (int i = 0; i < spawners.Length; i++)
                {
                    spawners[i].SetActive(true);
                }
            }
        }
    }

    public void btn_StartAttack()
    {
        if (GameManager.S.gameState != GameManager.GameState.playing) return;
        StartCoroutine(StartAttackCoroutine());
    }

    private IEnumerator StartAttackCoroutine()
    {
        yield return StartCoroutine(UIManager.S.FlashMiddleText("Enemies Incoming!!!", 0.25f, 2.5f));
        timeLeft = 0;
    }
}
