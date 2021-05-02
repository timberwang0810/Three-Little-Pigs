using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hut : MonoBehaviour
{
    public float maxHP;
    public int maxPigs;
    public Vector3 hutSpawnOffset;
    public Vector2 hutSpawnDirection;
    public float timeBetweenSpawn;
    private GameObject[] pigs;
    private float currHP;
    private int currPigs = 0;
    private bool isDestroyed = false;

    private void Start()
    {
        currHP = maxHP;
        pigs = new GameObject[maxPigs];
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

    public void OnPigEntered(GameObject pig)
    {
        pigs[currPigs] = pig;
        currPigs++;
        if (currPigs == maxPigs) GameManager.S.ResetLevel();
    }

    private IEnumerator ReleasePigs()
    {
        foreach (GameObject pig in pigs)
        {
            pig.GetComponent<SpriteRenderer>().enabled = true;
            pig.transform.position = transform.position + hutSpawnOffset;
            pig.GetComponent<Pig>().initialDirection = hutSpawnDirection;
            pig.GetComponent<Pig>().speed = 2;
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        GameManager.S.OnHutDestroyed();
        Destroy(this.gameObject, 1.0f);
    }


}
