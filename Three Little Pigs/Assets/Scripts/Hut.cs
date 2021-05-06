using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hut : MonoBehaviour
{
    public float maxHP;
    public Vector3 hutSpawnOffset;
    public Vector3 hutCameraPanOffset;
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
            Camera.main.GetComponent<CameraPan>().PanTo(transform.position + hutCameraPanOffset, 5);
            GetComponent<SpriteRenderer>().enabled = false;
            GameManager.S.OnHutDestroyed();
        }
    }

    public void OnPigEntered()
    {
        currPigs++;
        if (currPigs == pigs.Length) StartCoroutine(GameManager.S.ResetLevel());
    }

    public void OnPigsVictory()
    {
        StartCoroutine(VictoryCoroutine());
    }
 
    public IEnumerator ReleasePigs(bool isLoss)
    {
        foreach (GameObject pigObject in pigs)
        {
            GameObject pig = Instantiate(pigObject, transform.position + hutSpawnOffset, Quaternion.identity);
            if (isLoss) pig.GetComponent<Pig>().RunAroundForever(hutSpawnDirection, 3.0f, 2.0f);
            else pig.GetComponent<Pig>().SetCurrentDirection(hutSpawnDirection);
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        if (isLoss) UIManager.S.ShowLosingPanel();
        Destroy(this.gameObject, 1.0f);
    }

    private IEnumerator VictoryCoroutine()
    {
        yield return StartCoroutine(UIManager.S.FlashMiddleText("Is it Safe...Now??", 1.5f, 3.0f));
        Camera.main.GetComponent<CameraPan>().PanTo(transform.position + hutSpawnOffset + new Vector3(2, 0, 0), 5);
        for (int i = 0; i < pigs.Length; i++)
        {
            GameObject pig = Instantiate(pigs[i], transform.position + hutSpawnOffset + new Vector3(i,0,0), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            pig.GetComponent<Pig>().SetJump(true);
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        UIManager.S.ShowWinningPanel();
    }

}
