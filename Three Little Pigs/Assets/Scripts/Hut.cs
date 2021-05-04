﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hut : MonoBehaviour
{
    public float maxHP;
    public Vector3 hutSpawnOffset;
    public Vector2 hutSpawnDirection;
    public float timeBetweenSpawn;
    public GameObject[] pigs;
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
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(ReleasePigs());
        }
    }

    public void OnPigEntered()
    {
        currPigs++;
        if (currPigs == pigs.Length) GameManager.S.ResetLevel();
    }

    private IEnumerator ReleasePigs()
    {
        foreach (GameObject pigObject in pigs)
        {
            GameObject pig = Instantiate(pigObject, transform.position + hutSpawnOffset, Quaternion.identity);
            if (LevelManager.S.isFinalLevel) pig.GetComponent<Pig>().RunAroundForever(hutSpawnDirection, 3.0f, 2.0f);
            else pig.GetComponent<Pig>().initialDirection = hutSpawnDirection;
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        GameManager.S.OnHutDestroyed();
        Destroy(this.gameObject, 1.0f);
    }


}
